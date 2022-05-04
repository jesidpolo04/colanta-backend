using colanta_backend.App.Users.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.Users.Infraestructure
{
    using Microsoft.Extensions.Configuration;
    using Shared.Infraestructure;
    using System.Collections.Generic;
    using System.Linq;
    public class UsersEFRepository : Domain.UsersRepository
    {
        ColantaContext dbContext;
        public UsersEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }

        public async Task<User[]> getAllUsers()
        {
            EFUser[] efUsers = this.dbContext.Users.ToArray();
            List<User> users = new List<User>();
            foreach (EFUser efUser in efUsers)
            {
                users.Add(efUser.getUserFromEFUser());
            }
            return users.ToArray();
        }

        public async Task<User?> getUserByDocument(string document, string document_type = "CC")
        {
            var efUsers = this.dbContext.Users.Where(e => e.document == document && e.document_type == document_type);
            if(efUsers.ToArray().Length > 0)
            {
                return efUsers.First().getUserFromEFUser();
            }
            return null;
        }

        public async Task<User> saveUser(User user)
        {
            EFUser efUser = new EFUser();
            efUser.setEfUserFromUser(user);
            this.dbContext.Users.Add(efUser);
            this.dbContext.SaveChanges();
            return await this.getUserByDocument(user.document, user.document_type);
        }
    }
}
