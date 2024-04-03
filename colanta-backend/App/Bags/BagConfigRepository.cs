namespace colanta_backend.App.Bags
{
    using Microsoft.Extensions.Configuration;
    using App.Shared.Infraestructure;
    using System.Linq;

    public class BagConfigRepository
    {
        private ColantaContext DbContext;
        public BagConfigRepository(IConfiguration configuration)
        {
            DbContext = new ColantaContext(configuration);
        }

        public BagConfig? GetConfig(){
            var bagConfigs = DbContext.BagConfig.ToList();
            return bagConfigs.Count > 0 ? bagConfigs.First() : null;
        }

        public void CreateOrUpdateConfig(int bagSkuId, int capacityInGrams){
            var config = GetConfig();
            if(config == null){
                var newConfig = new BagConfig{
                    BagSkuId = bagSkuId,
                    CapacityInGrams = capacityInGrams
                };
                DbContext.Add(newConfig);
            }
            else{
                config.BagSkuId = bagSkuId;
                config.CapacityInGrams = capacityInGrams;
                DbContext.Update(config);
            }
            DbContext.SaveChanges();
        }
    }
}