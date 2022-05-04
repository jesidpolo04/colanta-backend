namespace colanta_backend.App.Users.Domain
{
    using System.Threading.Tasks;
    public interface UsersRepository
    {
        public Task<User> saveUser(User user);
        public Task<User?> getUserByDocument(string document, string document_type);
        public Task<User[]> getAllUsers();
    }
}
