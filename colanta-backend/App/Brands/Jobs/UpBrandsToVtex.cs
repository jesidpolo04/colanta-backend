namespace colanta_backend.App.Brands.Jobs
{
    using System.Threading.Tasks;
    using Brands.Application;
    using Brands.Domain;
    using Shared.Domain;
    using Shared.Application;
    using Shared.Infraestructure;
    public class UpBrandsToVtex
    {
        private BrandsVtexRepository vtexRepository;
        private BrandsRepository localRepository;

        public UpBrandsToVtex(BrandsRepository localRepository, BrandsVtexRepository vtexRepository)
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
        }

        public async Task<bool> Invoke()
        {
            GetNullVtexBrands getNullVtexBrand = new GetNullVtexBrands(this.localRepository);
            CreateVtexBrand createVtexBrand = new CreateVtexBrand(this.vtexRepository);
            UpdateBrand updateBrand = new UpdateBrand(this.localRepository);

            Brand[] nullVtexBrands = await getNullVtexBrand.Invoke();
            foreach(Brand nullVtexBrand in nullVtexBrands)
            {
                try
                {
                    Brand vtexBrand = await createVtexBrand.Invoke(nullVtexBrand);
                    nullVtexBrand.id_vtex = vtexBrand.id_vtex;
                    updateBrand.Invoke(nullVtexBrand);
                }
                catch
                {

                }
                
            }
            return true;
        }
    }
}


