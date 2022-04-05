namespace colanta_backend.App.Brands.Application
{
    using System.Threading.Tasks;
    using Brands.Domain;
    public class GetNullVtexBrands
    {
        private BrandsRepository repository;
        public GetNullVtexBrands(BrandsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Brand[]> Invoke()
        {
            return await this.repository.getNullVtexBrands();
        }
    }
}
