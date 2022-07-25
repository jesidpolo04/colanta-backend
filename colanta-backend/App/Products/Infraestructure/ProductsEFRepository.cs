using colanta_backend.App.Products.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.Products.Infraestructure
{
    using Shared.Infraestructure;
    using Categories.Infraestructure;
    using Brands.Infraestructure;
    using Shared.Domain;
    using Products.Domain;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductsEFRepository : Domain.ProductsRepository
    {
        private ColantaContext dbContext;
        public ProductsEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }
        public async Task<Product[]> getDeltaProducts(Product[] currentProducts)
        {
            List<string> currentIds = new List<string>();
            foreach (Product product in currentProducts)
            {
                currentIds.Add(product.siesa_id);
            }
            EFProduct[] efDeltaProducts = this.dbContext.Products.Where(product => !currentIds.Contains(product.siesa_id) && product.is_active == true).ToArray();
            List<Product> products = new List<Product>();
            foreach (EFProduct efDeltaProduct in efDeltaProducts)
            {
                products.Add(efDeltaProduct.getProductFromEfProduct());
            }
            return products.ToArray();
        }

        public async Task<Product> getProductBySiesaId(string siesaId)
        {
            var efProducts = this.dbContext.Products.Where(product => product.siesa_id == siesaId);
            if (efProducts.ToArray().Length > 0)
            {
                EFProduct efProduct = efProducts.First();
                //if(efProduct.brand_id == null)
                //{
                //    System.Console.WriteLine("Excepcion en get product");
                //    Product product = efProduct.getProductFromEfProduct();
                //    throw new BrandMustExistException($"Producto: {product.name} ({product.siesa_id}), no posee una marca", product);
                //}
                EFBrand efBrand = this.dbContext.Brands.Find(efProduct.brand_id);
                EFCategory efCategory = this.dbContext.Categories.Find(efProduct.category_id);
                efProduct.brand = efBrand;
                efProduct.category = efCategory;

                return efProduct.getProductFromEfProduct();
            }
            return null;
        }

        public async Task<Product> getProductByVtexId(int vtexId)
        {
            var efProducts = this.dbContext.Products.Where(product => product.vtex_id == vtexId);
            if (efProducts.ToArray().Length > 0)
            {
                return efProducts.First().getProductFromEfProduct();
            }
            return null;
        }

        public async Task<Product[]> getVtexNullProducts()
        {
            EFProduct[] efProducts = this.dbContext.Products.Where(product => product.vtex_id == null).ToArray();
            List<Product> products = new List<Product>();
            foreach (EFProduct efProduct in efProducts)
            {
                products.Add(efProduct.getProductFromEfProduct());
            }
            return products.ToArray();
        }

        public async Task<Product[]> getVtexProducts()
        {
            EFProduct[] efProducts = this.dbContext.Products.Where(product => product.vtex_id != null).ToArray();
            List<Product> products = new List<Product>();
            foreach (EFProduct efProduct in efProducts)
            {
                products.Add(efProduct.getProductFromEfProduct());
            }
            return products.ToArray();
        }

        public async Task<Product> saveProduct(Product product)
        {
            EFProduct efProduct = new EFProduct();
            //if (product.brand.id_siesa == null);
            //{
            //    System.Console.WriteLine("Excepcion en save product");
            //    throw new BrandMustExistException($"Producto: {product.name} ({product.siesa_id}), no posee una marca", product);
            //}
            EFBrand efBrand = this.dbContext.Brands.Where(brand => brand.id_siesa == product.brand.id_siesa).First();
            EFCategory efCategory = this.dbContext.Categories.Where(category => category.siesa_id == product.category.siesa_id).First();

            efProduct.setEfProductFromProduct(product);
            efProduct.category = efCategory;
            efProduct.brand = efBrand;

            this.dbContext.Add(efProduct);
            this.dbContext.SaveChanges();
            Product savedProduct = await this.getProductBySiesaId(product.siesa_id);
            return savedProduct;
        }

        public async Task<Product> updateProduct(Product product)
        {
            EFProduct efProduct = this.dbContext.Products.Find(product.id);
            efProduct.type = product.type;
            efProduct.siesa_id = product.siesa_id;
            efProduct.concat_siesa_id = product.concat_siesa_id;
            efProduct.vtex_id = product.vtex_id;
            efProduct.name = product.name;
            efProduct.ref_id = product.ref_id;
            efProduct.description = product.description;
            efProduct.is_active = product.is_active;
            efProduct.business = product.business;
            this.dbContext.SaveChanges();
            return product;
        }

        public async Task<Product[]> updateProducts(Product[] products)
        {
            foreach (Product product in products)
            {
                EFProduct efProduct = this.dbContext.Products.Find(product.id);
                efProduct.type = product.type;
                efProduct.siesa_id = product.siesa_id;
                efProduct.concat_siesa_id = product.concat_siesa_id;
                efProduct.vtex_id = product.vtex_id;
                efProduct.name = product.name;
                efProduct.ref_id = product.ref_id;
                efProduct.description = product.description;
                efProduct.is_active = product.is_active;
                efProduct.business = product.business;
            }
            dbContext.SaveChanges();
            return products;
        }
    }
}
