using Microsoft.AspNetCore.Mvc;
namespace colanta_backend.App.Auth.Controllers
{
    using System;
    using System.Collections.Generic;
    using colanta_backend.App.Auth.Services;
    using JWT;
    using JWT.Algorithms;
    using JWT.Serializers;
    using Microsoft.Extensions.Configuration;

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration configuration;

        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            var apiUsername = configuration["ApiUsername"];
            var apiPassword = configuration["ApiPassword"];

            if (request.username.Equals(apiUsername) && request.password.Equals(apiPassword))
            {
                try
                {
                    var service = new JWTService(configuration);
                    return Ok(new {
                        token = service.generateToken() 
                    });
                }
                catch (Exception exception)
                {
                    return StatusCode(500, new {
                        message = exception.Message,
                        stack = exception.StackTrace
                    });
                }
            }
            else
            {
                return BadRequest(new
                {
                    message = "Bad login."
                });
            }
        }
    }
}