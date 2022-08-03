namespace colanta_backend.App.Products.Domain
{
    using System.Collections.Generic;
    using Products.Domain;
    public interface IRenderProductsMail
    {
        void sendMail(List<Sku> loadedSkus, List<Sku> inactivatedSkus, List<Sku> failedSkus);
    }
}
