namespace colanta_backend.App.Users.Domain
{
    using System.Threading.Tasks;
    public interface UsersSiesaRepository
    {
        public Task<User> saveUser(User user);
    }
}
