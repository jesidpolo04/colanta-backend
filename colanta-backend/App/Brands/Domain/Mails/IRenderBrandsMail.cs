namespace colanta_backend.App.Brands.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IRenderBrandsMail
    {
        void sendMail(List<Brand> loadedBrands, List<Brand> inactivatedBrands, List<Brand> failBrands);

    }
}
