namespace colanta_backend.App.Brands.Jobs
{
    using Brands.Domain;
    using Brands.Application;
    using Shared.Domain;
    using Shared.Application;
    using System;
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
            GetAllBrands getAllBrands = new GetAllBrands(this.brandsLocalRepository);
            UpdateBrand updateBrand = new UpdateBrand(this.brandsLocalRepository);
            GetVtexBrandByVtexId getVtexBrandByVtexId = new GetVtexBrandByVtexId(this.brandsVtexRepository);

            Brand[] allLocalBrands = getAllBrands.Invoke();

            foreach (Brand localBrand in allLocalBrands)
            {
                try
                {
                    Brand vtexBrand = await getVtexBrandByVtexId.Invoke(localBrand);
                    if (vtexBrand.state != localBrand.state)
                    {
                        localBrand.state = vtexBrand.state;
                        updateBrand.Invoke(localBrand);
                    }
                }
                catch
                {

                }

            }

        }
    }
}
