namespace colanta_backend.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using App.Users.Domain;
    using App.Users.Application;
    using Controllers.Users;
   
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        UsersRepository localRepository;
        UsersSiesaRepository siesaRepository;

        public UsersController(UsersRepository localRepository, UsersSiesaRepository siesaRepository)
        {
            this.localRepository = localRepository;
            this.siesaRepository = siesaRepository;
        }

        [HttpGet]
        public object Get()
        {
            return new
            {
                message = "Hello User!"
            };
        }

        [HttpPost]
        public async Task<ActionResult<object>> Post(RequestUserDto requestUser)
        {
            SaveUser saveUser = new SaveUser(this.localRepository);
            SaveSiesaUser saveSiesaUser = new SaveSiesaUser(this.siesaRepository);

            User siesaUser = await saveSiesaUser.Invoke(requestUser.getUserDto());
            User localUser = await saveUser.Invoke(siesaUser);

            return new { 
                client_type = localUser.client_type
            };
            
        }

    }
}
