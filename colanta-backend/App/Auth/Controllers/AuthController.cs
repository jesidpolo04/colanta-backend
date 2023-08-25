using Microsoft.AspNetCore.Mvc;
namespace colanta_backend.App.Auth.Controllers
{
    using System.Collections.Generic;
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
        public void Login([FromBody] LoginRequest request)
        {
            var secret = configuration["JwtSecret"];
            var apiUsername = configuration["ApiUsername"];
            var apiPassword = configuration["ApiPassword"];

            if (request.username.Equals(apiUsername) && request.password.Equals(apiPassword))
            {
                var payload = new Dictionary<string, object>
                {
                    { "claim1", 0 },
                    { "claim2", "claim2-value" }
                };
                IJwtAlgorithm algorithm = new RS256Algorithm(System.Security.Cryptography.RSA.Create());
                IJsonSerializer serializer = new JsonNetSerializer();
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

                var token = encoder.Encode(payload, secret);
                Ok(new {
                    token
                });
            }else{
                BadRequest(new {
                    message = "Bad login."
                });
            }
        }
    }
}