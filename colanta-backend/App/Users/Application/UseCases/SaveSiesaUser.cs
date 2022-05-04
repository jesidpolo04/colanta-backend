namespace colanta_backend.App.Users.Application
{
    using Users.Domain;
    using System.Threading.Tasks;
    public class SaveSiesaUser
    {
        private UsersSiesaRepository siesaRepository;

        public SaveSiesaUser(UsersSiesaRepository siesaRepository)
        {
            this.siesaRepository = siesaRepository;
        }

        public async Task<User> Invoke(User user)
        {
            return await this.siesaRepository.saveUser(user);
        }
    }
}
