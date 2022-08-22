namespace colanta_backend.App.Users.Domain
{
    using System.Threading.Tasks;
    public interface UsersVtexRepository
    {
        Task<VtexUser> getByDocumentAndEmail(string document, string email);
        Task<VtexUser> getByEmail(string email);
        Task setCustomerClass(string vtexId, string customerClass);
    }
}
