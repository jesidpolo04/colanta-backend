namespace colanta_backend.App.Users.Domain
{
    using System.Threading.Tasks;
    public class RegisterUserService
    {
        private UsersSiesaRepository siesaRepository;
        private UsersVtexRepository vtexRepository;
        private UsersRepository localRepository;

        public RegisterUserService(UsersSiesaRepository siesaRepository, UsersVtexRepository vtexRepository, UsersRepository localRepository)
        {
            this.siesaRepository = siesaRepository;
            this.vtexRepository = vtexRepository;
            this.localRepository = localRepository;
        }

        public async Task registerUser
            (
                string document,
                string email
            )
        {
            VtexUser vtexUser = this.vtexRepository.getByDocumentAndEmail(document, email).Result;
            if(vtexUser == null) return;
            User user = VtexUserMapper.getUserFromVtexUser(vtexUser);
            user = await this.siesaRepository.saveUser(user);
            if(this.localRepository.getUserByEmail(email).Result == null)
            {
                await this.localRepository.saveUser(user);
            }
            this.vtexRepository.setCustomerClass(vtexUser.id, user.client_type).Wait();
        }   
    }
}
