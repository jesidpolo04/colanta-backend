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
    using Shared.Infraestructure;
    using Microsoft.Extensions.Configuration;
    public class UsersSiesaRepository : Domain.UsersSiesaRepository
    {
        private IConfiguration configuration;
        private HttpClient httpClient;
        private SiesaAuth siesaAuth;

        public UsersSiesaRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.httpClient = new HttpClient();
            this.siesaAuth = new SiesaAuth(configuration);
        }
        public async Task<User> saveUser(User user)
        {
            await this.setHeaders();
            string endpoint = "/api/ColantaWS/Clientes";
            SaveUserSiesaRequestDto request = new SaveUserSiesaRequestDto();

            request.documento = user.document;
            request.nombre = user.name;
            request.apellido1 = user.last_name;
            request.apellido2 = user.second_last_name;
            request.depto = user.department_code;
            request.ciudad = user.city_code;
            request.pais = user.country_code;
            request.telefono = user.telephone;
            request.celular = user.phone;
            request.correo = user.email;
            request.barrio = user.address;
            request.direccion = user.address;
            request.fechanacimiento = user.born_date;
            request.negocio = user.business;
            
            string requestJson = JsonSerializer.Serialize(request);
            HttpContent httpContent = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage siesaResponse = await this.httpClient.PostAsync(configuration["SiesaUrl"] + endpoint, httpContent);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(400, "Hubo un problema con Siesa, respondió con estado: " + siesaResponse.StatusCode);
            }
            string responseBody = await siesaResponse.Content.ReadAsStringAsync();
            SaveUserSiesaResponseDto responseDto = JsonSerializer.Deserialize<SaveUserSiesaResponseDto>(responseBody);
            user.client_type = responseDto.tipo_cliente;
            return user;
        }

        private async Task setHeaders()
        {
            this.httpClient.DefaultRequestHeaders.Remove("Authorization");
            this.httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await this.siesaAuth.getToken());
        }
    }
}
