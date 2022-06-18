﻿namespace colanta_backend.App.Products.Infraestructure
{
    using Products.Domain;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using Shared.Domain;
    using Microsoft.Extensions.Configuration;

    public class ProductsSiesaRepository : Domain.ProductsSiesaRepository
    {
        private IConfiguration configuration;
        private HttpClient httpClient;

        public ProductsSiesaRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.httpClient = new HttpClient();
        }

        public async Task<Product[]> getAllProducts()
        {
            string endpoint = "/productos";
            HttpResponseMessage siesaResponse = await this.httpClient.GetAsync(configuration["SiesaUrl"] + endpoint);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(((int)siesaResponse.StatusCode), "Falló la petición a siesa con estado: " + siesaResponse.StatusCode);
            }
            string siesaBodyResponse = await siesaResponse.Content.ReadAsStringAsync();
            SiesaProductsDto siesaProductsDto = JsonSerializer.Deserialize<SiesaProductsDto>(siesaBodyResponse);
            List<Product> products = new List<Product>();
            foreach (SiesaProductDto siesaProductDto in siesaProductsDto.productos)
            {
                products.Add(siesaProductDto.getProductFromDto());
            }
            return products.ToArray();
        }

        public async Task<Sku[]> getAllSkus()
        {
            List<Sku> skus = new List<Sku>();
            Product[] allProducts = await this.getAllProducts();
            foreach(Product product in allProducts)
            {
                foreach(Sku sku in product.skus)
                {
                    skus.Add(sku);
                }
            }
            return skus.ToArray();
        }
    }
}