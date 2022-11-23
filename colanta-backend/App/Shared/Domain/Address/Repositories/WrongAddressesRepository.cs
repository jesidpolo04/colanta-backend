namespace colanta_backend.App.Shared.Domain
{
    using System.Threading.Tasks;
    public interface WrongAddressesRepository
    {
        Task<WrongAddress[]> getAllWrongAddresses();
        Task<WrongAddress?> getWrongAddress(string country, string department, string city);
    }
}
