namespace colanta_backend.App.Brands.Application
{
    using System.Threading.Tasks;
    using System;
    using Brands.Domain;
    using Shared.Domain;
    using Shared.Application;

    public class CreateVtexBrand
    {
        private BrandsVtexRepository _brandsVtexRepository;
        private int _numberOfTry;
        public CreateVtexBrand(BrandsVtexRepository brandsVtexRepository)
        {
            this._numberOfTry = 5;
            this._brandsVtexRepository = brandsVtexRepository;
        }

        public async Task<Brand?> Invoke(Brand brand)
        {
            try
            {
                this._brandsVtexRepository.changeEnviroment(brand.business);
                Brand vtexBrand = await this._brandsVtexRepository.saveBrand(brand);
                return vtexBrand;
            }
            catch(VtexException vtexExcepcion)
            {
                Brand? vtexBrand = null;
                for(int i = 1; i <= this._numberOfTry; i++)
                {
                    try
                    {
                        vtexBrand = await this._brandsVtexRepository.saveBrand(brand);
                        break;
                    }catch(VtexException tryException)
                    {
                        if(i == (this._numberOfTry))
                        {
                            throw tryException;
                        }
                    }
                }
                return vtexBrand;
            }
        }
    }
}
