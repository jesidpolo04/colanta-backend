namespace colanta_backend.App.Shared.Domain
{
    public class AddressCorrector
    {
        public static string correctCityIfIsWrong(string country, string state, string city)
        {
            var wrongAddresses = WrongAddresses.get();
            foreach(WrongAddress wrongAddress in wrongAddresses)
            {
                if (wrongAddress.isWrongAddress(country, state, city))
                    return wrongAddress.getRightCity();
            }
            return city;
        }

        public static string correctStateIfIsWrong(string country, string state, string city)
        {
            var wrongAddresses = WrongAddresses.get();
            foreach(WrongAddress wrongAddress in wrongAddresses)
            {
                if (wrongAddress.isWrongAddress(country, state, city))
                    return wrongAddress.getRightState();
            }
            return state;
        }

        public static string correctCountryIfIsWrong(string country, string state, string city)
        {
            var wrongAddresses = WrongAddresses.get();
            foreach(WrongAddress wrongAddress in wrongAddresses)
            {
                if (wrongAddress.isWrongAddress(country, state, city))
                    return wrongAddress.getRightCountry();
            }
            return country;
        }
    }
}
