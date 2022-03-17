namespace colanta_backend.App.Brands.Jobs
{
    using App.Brands.Application;
    using App.Brands.Domain;
    using App.Brands.Infraestructure;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    public class RenderBrands
    {
        private GetBrandBySiesaId getBrandBySiesaId;
        private CreateBrand createBrand;
        private UpdateBrand updateBrand;
        private HttpClient httpClient;
        private string url = "https://colanta.myvtex.com/";
        private string endpoint = "api/catalog/pvt/brand";
        private string apiToken = "FIROYQZUHUNDYJAEFJXKOHXRTUNTNSERIPTKGTSVGVFFVCNJOSSHOIOYLAUECFHPPWUIQQXLRCDTCSWRGDEUZXCABUYGYOSBNPPYHETYVHQMEUWSEDXZAMQSUUHWRRMD";
        private string apiKey = "vtexappkey-colanta-CNANOI";

        public RenderBrands()
        {
            this.httpClient = new HttpClient();
            this.getBrandBySiesaId = new GetBrandBySiesaId(new EFBrandsRepository());
            this.createBrand = new CreateBrand(new EFBrandsRepository(), new VtexBrandsRepository());
            this.updateBrand = new UpdateBrand(new EFBrandsRepository());
        }

        public async void Invoke()
        {
            // request info from Siesa
            var responseSiesaBrands = await this.httpClient.GetAsync("http://localhost:3000/marcas");
            string siesaBrandsBody = await responseSiesaBrands.Content.ReadAsStringAsync();
            var siesaBrands = JsonSerializer.Deserialize<List<SiesaBrandDTO>>(siesaBrandsBody).ToArray();

            //Set headers for vtex
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppToken", this.apiToken);
            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppKey", this.apiKey);

            foreach (SiesaBrandDTO siesaBrand in siesaBrands)
            {
                Brand localBrand = this.getBrandBySiesaId.Invoke(siesaBrand.id);

                if(localBrand != null)
                {
                    //If Brand Exists Locally: Write...
                    await Console.Out.WriteAsync("Ya existe la marca con siesa Id: " + localBrand.id_siesa + "\n");
                }
                else
                {
                    //Create brand if no exists
                    localBrand = this.createBrand.Invoke(new Brand(
                            id_siesa: siesaBrand.id,
                            name: siesaBrand.nombre,
                            id: Convert.ToInt16(siesaBrand.id)
                        ));
                    Console.Out.Write("Se creó localmente la marca con id Siesa: " + siesaBrand.id + "\n");

                    //Create brand in VTEX
                    
                    string jsonContent = JsonSerializer.Serialize(new
                    {
                        Name = siesaBrand.nombre
                    });
                    HttpContent content = new StringContent(jsonContent, encoding:System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage responseVtex = await this.httpClient.PostAsync(this.url + this.endpoint, content);
                    string responseBodyVtex = await responseVtex.Content.ReadAsStringAsync();
                    Console.Out.Write("Body Vtex: "+responseBodyVtex + "\n");
                    VtexBrandDTO vtexBrand = JsonSerializer.Deserialize<VtexBrandDTO>(responseBodyVtex);
                    Console.Out.WriteLine("Se creó la marca con Id Siesa: "+siesaBrand.id+" En VTEX con id vtex: "+vtexBrand.Id);

                    //Update brand Locally
                    localBrand.id_vtex = vtexBrand.Id;
                    Brand updatedLocalBrand = this.updateBrand.Invoke(localBrand);
                    Console.Out.WriteLine("Se actualizó el id vtex ("+localBrand.id_vtex+") para marca con Siesa Id: " + localBrand.id_siesa);
                }
            }
        }
    }
}
