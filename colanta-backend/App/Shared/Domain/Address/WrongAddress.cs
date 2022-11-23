namespace colanta_backend.App.Shared.Domain
{
    using System.Collections.Generic;
    public class WrongAddress
    {
        private string vtexCountry;
        private string siesaCountry;
        private string vtexCity;
        private string siesaCity;
        private string vtexDepartment;
        private string siesaDepartment;

        public WrongAddress(string vtexCountry, string vtexDepartment, string vtexCity, string siesaCountry, string siesaDepartment, string siesaCity)
        {
            this.vtexCountry = vtexCountry;
            this.siesaCountry = siesaCountry;
            this.vtexCity = vtexCity;
            this.siesaCity = siesaCity;
            this.vtexDepartment = vtexDepartment;
            this.siesaDepartment = siesaDepartment;
        }

        public bool isWrongAddress(string country, string department, string city)
        {
            if (country == this.vtexCountry && department == this.vtexDepartment && city == this.vtexCity) return true;
            else return false;
        }

        public string getVtexCountry()
        {
            return this.vtexCountry;
        }

        public string getSiesaCountry()
        {
            return this.siesaCountry;
        }

        public string getVtexCity()
        {
            return this.vtexCity;
        }

        public string getSiesaCity()
        {
            return this.siesaCity;
        }

        public string getVtexDeparment()
        {
            return this.vtexDepartment;
        }

        public string getSiesaDepartment()
        {
            return this.siesaDepartment;
        }
    }
}
