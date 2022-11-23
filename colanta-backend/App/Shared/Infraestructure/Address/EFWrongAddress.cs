namespace colanta_backend.App.Shared.Infraestructure
{
    using Shared.Domain;
    public class EFWrongAddress
    {
        public int id;
        public string vtexCountry;
        public string siesaCountry;
        public string vtexCity;
        public string siesaCity;
        public string vtexDepartment;
        public string siesaDepartment;

        public void setEfWrongAddress(WrongAddress wrongAddress)
        {
            this.vtexCountry = wrongAddress.getVtexCountry();
            this.siesaCountry = wrongAddress.getSiesaCountry();
            this.vtexCity = wrongAddress.getVtexCity();
            this.siesaCity = wrongAddress.getSiesaCity();
            this.vtexDepartment = wrongAddress.getVtexDeparment();
            this.siesaDepartment = wrongAddress.getSiesaDepartment();
        }

        public WrongAddress getWrongAddress()
        {
            return new WrongAddress(
                    this.vtexCountry,
                    this.vtexDepartment,
                    this.vtexCity,
                    this.siesaCountry,
                    this.siesaDepartment,
                    this.siesaCity
                );
        }
    }
}
