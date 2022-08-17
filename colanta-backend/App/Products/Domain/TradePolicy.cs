namespace colanta_backend.App.Products.Domain
{
    public class TradePolicy
    {
        public int id;
        public string name;

        public TradePolicy(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
