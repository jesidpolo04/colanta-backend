namespace colanta_backend.App.Products.Infraestructure
{
    using Shared.Infraestructure;
    using Shared.Domain;
    using Products.Domain;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public class SkusEFRepository : Domain.SkusRepository
    {
        private ColantaContext dbContext;
        public SkusEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }

        public async Task<Sku[]> getDeltaSkus(Sku[] currentSkus)
        {
            List<string> currentIds = new List<string>();
            foreach (Sku product in currentSkus)
            {
                currentIds.Add(product.siesa_id);
            }
            EFSku[] efDeltaSkus = this.dbContext.Skus.Where(sku => !currentIds.Contains(sku.siesa_id) && sku.is_active == true).ToArray();
            List<Sku> skus = new List<Sku>();
            foreach (EFSku efDeltaSku in efDeltaSkus)
            {
                skus.Add(efDeltaSku.GetSkuFromEfSku());
            }
            return skus.ToArray();
        }

        public async Task<Sku> getSkuByConcatSiesaId(string concatSiesaId)
        {
            var efSkus = this.dbContext.Skus.Where(sku => sku.concat_siesa_id == concatSiesaId);
            if (efSkus.ToArray().Length > 0)
            {
                EFSku efSku = efSkus.First();
                EFProduct efProduct = this.dbContext.Products.Find(efSku.product_id);
                efSku.product = efProduct;
                return efSku.GetSkuFromEfSku();
            }
            return null;
        }

        public async Task<Sku> getSkuBySiesaId(string siesaId)
        {
            var efSkus = this.dbContext.Skus.Where(sku => sku.siesa_id == siesaId);
            if (efSkus.ToArray().Length > 0)
            {
                EFSku efSku = efSkus.First();
                EFProduct efProduct = this.dbContext.Products.Find(efSku.product_id);
                efSku.product = efProduct;
                return efSku.GetSkuFromEfSku();
            }
            return null;
        }

        public async Task<Sku> getSkuByVtexId(int vtexId)
        {
            var efSkus = this.dbContext.Skus.Where(sku => sku.vtex_id == vtexId);
            if (efSkus.ToArray().Length > 0)
            {
                EFSku efSku = efSkus.First();
                EFProduct efProduct = this.dbContext.Products.Find(efSku.product_id);
                efSku.product = efProduct;
                return efSkus.First().GetSkuFromEfSku();
            }
            return null;
        }

        public async Task<Sku[]> getVtexNullSkus()
        {
            EFSku[] efSkus = this.dbContext.Skus.Where(sku => sku.vtex_id == null).ToArray();
            List<Sku> skus = new List<Sku>();
            foreach(EFSku efSku in efSkus)
            {
                skus.Add(efSku.GetSkuFromEfSku());
            }
            return skus.ToArray();
        }

        public async Task<Sku[]> getVtexSkus()
        {
            EFSku[] efSkus = this.dbContext.Skus.Where(sku => sku.vtex_id != null)
                .Include(s => s.product)
                .ToArray();
            List<Sku> skus = new List<Sku>();
            foreach (EFSku efSku in efSkus)
            {
                skus.Add(efSku.GetSkuFromEfSku());
            }
            return skus.ToArray();
        }

        public async Task<Sku> saveSku(Sku sku)
        {
            EFSku efSku = new EFSku();
            EFProduct efProduct = this.dbContext.Products.Where(e => e.siesa_id == sku.product.siesa_id).First();
            efSku.setEfSkuFromSku(sku);
            efSku.product = efProduct;
            this.dbContext.Add(efSku);
            this.dbContext.SaveChanges();
            return await this.getSkuBySiesaId(sku.siesa_id);
        }

        public async Task<Sku> updateSku(Sku sku)
        {
            EFSku efSku = this.dbContext.Skus.Find(sku.id);

            efSku.siesa_id = sku.siesa_id;
            efSku.concat_siesa_id = sku.concat_siesa_id;
            efSku.vtex_id = sku.vtex_id;
            efSku.name = sku.name;
            efSku.description = sku.description;
            efSku.ref_id = sku.ref_id;
            efSku.packaged_height = sku.packaged_height;
            efSku.packaged_length = sku.packaged_length;
            efSku.packaged_width = sku.packaged_width;
            efSku.packaged_weight_kg = sku.packaged_weight_kg;
            efSku.is_active = sku.is_active;
            efSku.measurement_unit = sku.measurement_unit;
            efSku.unit_multiplier = sku.unit_multiplier;

            this.dbContext.SaveChanges();
            return sku;
        }

        public async Task<Sku[]> updateSkus(Sku[] skus)
        {
            foreach (Sku sku in skus)
            {
                EFSku efSku = this.dbContext.Skus.Find(sku.id);
                efSku.siesa_id = sku.siesa_id;
                efSku.concat_siesa_id = sku.concat_siesa_id;
                efSku.vtex_id = sku.vtex_id;
                efSku.name = sku.name;
                efSku.description = sku.description;
                efSku.ref_id = sku.ref_id;
                efSku.packaged_height = sku.packaged_height;
                efSku.packaged_length = sku.packaged_length;
                efSku.packaged_width = sku.packaged_width;
                efSku.packaged_weight_kg = sku.packaged_weight_kg;
                efSku.is_active = sku.is_active;
                efSku.measurement_unit = sku.measurement_unit;
                efSku.unit_multiplier = sku.unit_multiplier;
            }
            this.dbContext.SaveChanges();
            return skus;
        }
    }
}
