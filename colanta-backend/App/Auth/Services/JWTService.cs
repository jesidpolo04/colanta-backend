namespace colanta_backend.App.Auth.Services
{
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using JWT;
    using JWT.Algorithms;
    using JWT.Serializers;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
    using JWT.Exceptions;

    public class JWTService
    {
        private IConfiguration configuration;
        public JWTService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string generateToken()
        {
            var secret = configuration["JwtSecret"];
            var payload = new Dictionary<string, object>
                    {
                        { "claim1", 0 },
                        { "claim2", "claim2-value" }
                    };
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }

        public bool validToken(string token)
        {
            var secret = configuration["JwtSecret"];

            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
            try
            {
                var json = decoder.Decode(token, secret, true);
                return true;
            }
            catch (TokenNotYetValidException)
            {
                return false;
            }
            catch (TokenExpiredException)
            {
                return false;
            }
            catch (SignatureVerificationException)
            {
                return false;
            }
        }
    }
}