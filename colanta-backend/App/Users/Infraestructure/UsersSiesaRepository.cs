using colanta_backend.App.Users.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.Users.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Users.Domain;
    using Shared.Domain;
    using Microsoft.Extensions.Configuration;
    public class UsersSiesaRepository : Domain.UsersSiesaRepository
    {
        private IConfiguration configuration;
        private HttpClient httpClient;

        public UsersSiesaRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.httpClient = new HttpClient();
        }
        public async Task<User> saveUser(User user)
        {
            string endpoint = "/clientes";
            HttpResponseMessage siesaResponse = await this.httpClient.GetAsync(configuration["SiesaUrl"] + endpoint);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(400, "Hubo un problema con Siesa, respondió con estado: " + siesaResponse.StatusCode);
            }
            string responseBody = await siesaResponse.Content.ReadAsStringAsync();
            SaveUserSiesaResponseDto responseDto = JsonSerializer.Deserialize<SaveUserSiesaResponseDto>(responseBody);
            user.client_type = responseDto.tipo_cliente;
            return user;
        }
    }
}
