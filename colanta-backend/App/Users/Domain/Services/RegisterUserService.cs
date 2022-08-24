namespace colanta_backend.App.Users.Domain
{
    using System.Threading.Tasks;
    using System.Text.RegularExpressions;
    using System;
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
                string vtexId,
                string country,
                string departament,
                string city
            )
        {
            VtexUser vtexUser = this.vtexRepository.getByVtexId(vtexId).Result;
            if(vtexUser == null) return;
            User user = VtexUserMapper.getUserFromVtexUser(vtexUser);
            user.country_code = "Colombia";
            user.department_code = departament;
            user.city_code = city;
            user.born_date = DateTime.Now.ToString("yyyyMMdd");
            user = await this.siesaRepository.saveUser(user);
            if(this.localRepository.getUserByEmail(vtexUser.email).Result == null)
            {
                await this.localRepository.saveUser(user);
            }
            this.vtexRepository.setCustomerClass(vtexUser.id, user.client_type).Wait();
        }   
    }
}
