namespace colanta_backend.App.Shared.Domain
{
    using System.Collections.Generic;
    public class WrongAddress
    {
        private string country;
        private string rightCountry;
        private string city;
        private string rightCity;
        private string state;
        private string rightState;

        public WrongAddress(string country, string state, string city, string rightCountry, string rightState, string rightCity)
        {
            this.country = country;
            this.rightCountry = rightCountry;
            this.city = city;
            this.rightCity = rightCity;
            this.state = state;
            this.rightState = rightState;
        }

        public bool isWrongAddress(string country, string state, string city)
        {
            if (country == this.country && state == this.state && city == this.city) return true;
            else return false;
        }

        public string getRightCountry()
        {
            return this.rightCountry;
        }

        public string getRightCity()
        {
            return this.rightCity;
        }

        public string getRightState()
        {
            return this.rightState;
        }
    }
}
