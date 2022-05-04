namespace colanta_backend.App.Products.Application
{
    using Products.Domain;
    using System.Threading.Tasks;
    public class GetProductBySiesaId
    {
        private ProductsRepository localReposiory;
        public GetProductBySiesaId(ProductsRepository localReposiory)
        {
            this.localReposiory = localReposiory;
        }

        public async Task<Product> Invoke(string siesaId)
        {
            return await this.localReposiory.getProductBySiesaId(siesaId);
        }
    }
}
