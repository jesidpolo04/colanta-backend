namespace colanta_backend.App.Users.Application
{
    using Users.Domain;
    using System.Threading.Tasks;
    public class SaveUser
    {
        private UsersRepository localRepository;
        public SaveUser(UsersRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<User> Invoke(User user)
        {
            await this.localRepository.saveUser(user);
            return user;
        }

    }
}
