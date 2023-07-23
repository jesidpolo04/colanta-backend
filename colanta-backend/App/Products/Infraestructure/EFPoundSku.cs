
namespace colanta_backend.App.Products.Infraestructure
{
    using App.Products.Domain;

    public class EFPoundSku
    {
        public string siesaId { get; set; }
        public string name { get; set; }

        public PoundSku getPoundSku()
        {
            return new PoundSku(this.siesaId, this.name);
        }

        public void setEFPoundSku(PoundSku poundSku)
        {
            this.siesaId = poundSku.siesaId;
            this.name = poundSku.name;
        }
    }
}
