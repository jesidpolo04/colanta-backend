namespace colanta_backend.App.Brands.Infraestructure
{
    using Brands.Domain;
    using Shared.Domain;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public class RenderBrandsMail : IRenderBrandsMail
    {
        EmailSender emailSender;
        string subject;
        
        public RenderBrandsMail(EmailSender emailSender)
        {
            this.emailSender = emailSender;
            this.subject = "Renderizado de marcas";
        }
        public void sendMail(List<Brand> loadedBrands, List<Brand> inactivatedBrands, List<Brand> failedBrands)
        {
            bool sendMail = false;
            sendMail = loadedBrands.Count > 0 ? true : sendMail;
            sendMail = inactivatedBrands.Count > 0 ? true : sendMail;
            sendMail = failedBrands.Count > 0 ? true : sendMail;
            if (sendMail)
            {
                RenderBrandsMailModel model = new RenderBrandsMailModel(loadedBrands, inactivatedBrands, failedBrands);
                this.emailSender.SendEmail(this.subject, "./App/Brands/Infraestructure/Mails/RenderBrandsMail.cshtml", model, EmailAddresses.Administrative);
            }
        }
    }
}
