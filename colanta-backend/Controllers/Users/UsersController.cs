namespace colanta_backend.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using System;
    using App.Users.Domain;
    using App.Users.Application;
    using Controllers.Users;
    using App.Shared.Domain;
    using Microsoft.AspNetCore.Cors;

    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private UsersRepository localRepository;
        private UsersSiesaRepository siesaRepository;
        private EmailSender emailSender;
        private ILogger logger;

        public UsersController(UsersRepository localRepository, UsersSiesaRepository siesaRepository, EmailSender emailSender, ILogger logger)
        {
            this.localRepository = localRepository;
            this.siesaRepository = siesaRepository;
            this.emailSender = emailSender;
            this.logger = logger;
        }

        [HttpPost]
        [EnableCors("Ecommerce")]
        [Route("remove")]
        public async Task<ActionResult> Remove(RequestRemoveUserDto request)
        {
            try
            {
                var useCase = new SendRemoveUserRequest(this.emailSender);
                useCase.Invoke(request.email, request.firstName, request.lastName, request.document, request.documentType);
                return Ok();
            }
            catch (Exception exception)
            {
                await this.logger.writelog(exception);
                throw exception;
            }
            
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
