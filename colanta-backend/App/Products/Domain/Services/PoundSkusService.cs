namespace colanta_backend.App.Products.Domain
{
    public class PoundSkusService
    {
        private SkusRepository repository;
        public PoundSkusService(SkusRepository repository)
        {
            this.repository = repository;
        }

        public bool isPoundSku(string siesaId)
        {
            var poundSkus = this.repository.getAllPoundSkus().Result;
            foreach(PoundSku sku in poundSkus) 
            {
                if(siesaId == sku.siesaId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
