using Microsoft.EntityFrameworkCore.Metadata;

namespace colanta_backend.App.Products.Domain
{
    public class PoundSku
    {
        public PoundSku(string siesaId, string name)
        {
            this.siesaId = siesaId;
            this.name = name;
        }

        public string siesaId { get; set; }
        public string name { get; set; }
    }
}
