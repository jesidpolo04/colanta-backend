namespace colanta_backend.App.Shared.Infraestructure
{
    using System.Text.Json;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using Shared.Domain;
    using System.Threading.Tasks;

    public class WrongAddressesEFReppository : WrongAddressesRepository
    {
        private ColantaContext dbContext;

        public WrongAddressesEFReppository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }

        public async Task<WrongAddress[]> getAllWrongAddresses()
        {
            var efWrongAddresses = this.dbContext.WrongAddresses.ToArray();
            List<WrongAddress> wrongAddresses = new List<WrongAddress>();
            foreach (var efWrongAddress in efWrongAddresses)
            {
                wrongAddresses.Add(efWrongAddress.getWrongAddress());
            }
            return wrongAddresses.ToArray();
        }

        public async Task<WrongAddress> getWrongAddress(string country, string department, string city)
        {
            var efWrongAddresses = this.dbContext.WrongAddresses
                .Where(wrongAddress =>
                        wrongAddress.vtexCountry == country &&
                        wrongAddress.vtexDepartment == department &&
                        wrongAddress.vtexCity == city);
            if(efWrongAddresses.ToArray().Length == 0)
            {
                return null;
            }
            return efWrongAddresses.First().getWrongAddress();
        }
    }
}
