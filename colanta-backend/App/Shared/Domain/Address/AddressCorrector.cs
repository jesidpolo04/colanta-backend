namespace colanta_backend.App.Shared.Domain
{
    public class AddressCorrector
    {
        private WrongAddressesRepository repository;
        public AddressCorrector(WrongAddressesRepository repository)
        {
            this.repository = repository;
        }

        public string correctCityIfIsWrong(string country, string state, string city)
        {

            var wrongAddresses = this.repository.getAllWrongAddresses().Result;
            foreach(WrongAddress wrongAddress in wrongAddresses)
            {
                if (wrongAddress.isWrongAddress(country, state, city))
                    return wrongAddress.getSiesaCity();
            }
            return city;
        }

        public string correctStateIfIsWrong(string country, string state, string city)
        {
            var wrongAddresses = this.repository.getAllWrongAddresses().Result;
            foreach (WrongAddress wrongAddress in wrongAddresses)
            {
                if (wrongAddress.isWrongAddress(country, state, city))
                    return wrongAddress.getSiesaDepartment();
            }
            return state;
        }

        public string correctCountryIfIsWrong(string country, string state, string city)
        {
            var wrongAddresses = this.repository.getAllWrongAddresses().Result;
            foreach (WrongAddress wrongAddress in wrongAddresses)
            {
                if (wrongAddress.isWrongAddress(country, state, city))
                    return wrongAddress.getSiesaCountry();
            }
            return country;
        }
    }
}
