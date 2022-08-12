namespace colanta_backend.App.Promotions.Infraestructure
{
    public class PromotionTypes
    {
        public static string BONO = "forThePriceOf";
        public static string REGALO = "buyAndWin";
        public static string KIT = "combo";
        public static string PORCENTUAL = "regular";
        public static string NOMINAL = "regular";

        public static string Map(string type)
        {
            switch (type)
            {
                case "bono":
                    return BONO;
                case "regalo":
                    return REGALO;
                case "kit":
                    return KIT;
                case "porcentual":
                    return PORCENTUAL;
                case "nominal":
                    return NOMINAL;
                default:
                    return "regular";
            }
        }
    }
}
