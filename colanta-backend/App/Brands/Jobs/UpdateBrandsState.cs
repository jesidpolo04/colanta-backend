namespace colanta_backend.App.Brands.Jobs
{
    using Brands.Domain;
    using Brands.Application;
    using Brands.Infraestructure;
    using Shared.Domain;
    using Shared.Application;
    public class UpdateBrandsState
    {
        BrandsRepository brandsLocalRepository;
        BrandsVtexRepository brandsVtexRepository;
        public UpdateBrandsState(BrandsRepository brandsLocalRepository, BrandsVtexRepository brandsVtexRepository)
        {
            this.brandsLocalRepository = brandsLocalRepository;
            this.brandsVtexRepository = brandsVtexRepository;
        }

        public async void Invoke()
        {
            //traer todas las marcas locales
            GetAllBrands getAllBrands = new GetAllBrands(this.brandsLocalRepository);
            UpdateBrand updateBrand = new UpdateBrand(this.brandsLocalRepository);
            GetVtexBrandByVtexId getVtexBrandByVtexId = new GetVtexBrandByVtexId(this.brandsVtexRepository);
            

            Brand[] allLocalBrands = getAllBrands.Invoke();

            //recorrer las marcas locales
            //consultar cada marca individual en vtex y confirmar el estado
            //si el estado es distinto, actualizar el estado
            foreach (Brand localBrand in allLocalBrands)
            {
                Brand vtexBrand = await getVtexBrandByVtexId.Invoke(localBrand);
                if(vtexBrand.state != localBrand.state)
                {
                    localBrand.state = vtexBrand.state;
                    updateBrand.Invoke(localBrand);
                }
            }

        }
    }
}
