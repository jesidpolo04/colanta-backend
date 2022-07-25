namespace colanta_backend.App.Brands.Jobs
{
    using Shared.Domain;
    using Brands.Domain;
    using System.Collections.Generic;
    using System;
    public class RenderBrandsMail
    {
        private HtmlWriter htmlWriter;
        private EmailSender emailSender;
        public string emailTitle = "Renderizado de Marcas";
        public string emailSubtitle = "Middleware Colanta";
        public DateTime dateTime;
        public RenderBrandsMail(EmailSender emailSender)
        {
            this.htmlWriter = new HtmlWriter();
            this.emailSender = emailSender;
            this.dateTime = DateTime.Now;
        }
        public void sendMail(Brand[] inactiveBrands, Brand[] failedLoadBrands, Brand[] loadBrands)
        {
            bool sendEmail = false;
            string body = "";

            if (loadBrands.Length > 0)
            {
                sendEmail = true;
                body += htmlWriter.h("5", "Marcas creadas en VTEX") + "\n";
                List<string> loadBrandsInfo = new List<string>();
                foreach (Brand loadBrand in loadBrands)
                {
                    loadBrandsInfo.Add($"{loadBrand.name}, con SIESA id: {loadBrand.id_siesa} y VTEX id: {loadBrand.id_vtex}");
                }
                body += htmlWriter.ul(loadBrandsInfo.ToArray());
                body += "\n";
            }

            if (inactiveBrands.Length > 0)
            {
                sendEmail = true;
                body += htmlWriter.h("5", "Marcas Inactivas en VTEX") + "\n";
                List<string> inactiveBrandsInfo = new List<string>();
                foreach (Brand inactiveBrand in inactiveBrands)
                {
                    inactiveBrandsInfo.Add($"{inactiveBrand.name}, con SIESA id: {inactiveBrand.id_siesa} y VTEX id: {inactiveBrand.id_vtex}");
                }
                body += htmlWriter.ul(inactiveBrandsInfo.ToArray());
                body += "\n";
            }
           
            if(failedLoadBrands.Length > 0)
            {
                sendEmail = true;
                body += htmlWriter.h("5", "Marcas que fallaron al cargarse a VTEX") + "\n";
                List<string> failedLoadBrandsInfo = new List<string>();
                foreach (Brand failedLoadBrand in failedLoadBrands)
                {
                    failedLoadBrandsInfo.Add($"{failedLoadBrand.name}, con SIESA id: {failedLoadBrand.id_siesa}");
                }
                body += htmlWriter.ul(failedLoadBrandsInfo.ToArray());
                body += "\n";
            }

            if (sendEmail)
            {
                this.emailSender.SendEmail(this.emailTitle, body);
            }
        }
    }
}
