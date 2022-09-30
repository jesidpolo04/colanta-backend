namespace colanta_backend.App.Shared.Domain
{
    using System.Collections.Generic;
    public class WrongAddresses
    {
        private static List<WrongAddress> addresses = new List<WrongAddress>();
        public static List<WrongAddress> get()
        {
            addresses.Add(new WrongAddress(
                "COL", 
                "Risaralda", 
                "Colombia",

                "COL", 
                "Risaralda", 
                "Pereira"
            ));
            addresses.Add(new WrongAddress(
                "COL",
                "Bolívar",
                "Provincia de Cartagena",

                "COL", 
                "Bolívar", 
                "Cartagena"
            ));

            return addresses;
        }
    }
}
