using System;
using System.Collections.Generic;
using System.Linq;
using colanta_backend.App.Shared.Infraestructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace colanta_backend.App.PriceTables
{
    public class PriceTablesRepository
    {
        private ColantaContext _Context;
        private ILogger _Logger;
        public PriceTablesRepository(IConfiguration configuration, ILogger<PriceTablesRepository> logger)
        {
            _Context = new ColantaContext(configuration);
            _Logger = logger;
        }

        public void Save(PriceTable priceTable)
        {
            try
            {
                _Context.PriceTables.Add(priceTable);
                _Context.SaveChanges();
            }
            catch (Exception exception)
            {
                _Logger.LogError($"Error al guardar la price table '{priceTable.Name}' \nError: {exception.Message} \nStack: {exception.StackTrace}");
            }
        }

        public PriceTable? GetByName(string tableName)
        {
            try
            {
                var results = _Context.PriceTables.Where(priceTable => priceTable.Name.Equals(tableName)).ToList();
                return results.Count > 0 ? results.First() : null;
            }
            catch (Exception exception)
            {
                _Logger.LogError($"Error al consultar por la tabla con nombre: '{tableName}' \nError: {exception.Message} \nStack: {exception.StackTrace}");
                return null;
            }
        }

        public List<PriceTable> GetAll(){
            try
            {
                return _Context.PriceTables.ToList();
            }
            catch (Exception exception)
            {
                _Logger.LogError($"Error al consultar todas las tablas \nError: {exception.Message} \nStack: {exception.StackTrace}");
                return  new List<PriceTable>();
            }
        }

        public FixedPrice[] GetFixedPricesBySku(int vtexSkuId){
            return _Context.FixedPrices
            .Include( fixedPrice => fixedPrice.PriceTable )
            .Where( fixedPrice => fixedPrice.VtexSkuId == vtexSkuId ).ToArray();
        }

        public void SaveFixedPrices(FixedPrice[] fixedPrices)
        {
            try
            {
                _Context.FixedPrices.AddRange(fixedPrices);
                _Context.SaveChanges();
            }
            catch (Exception exception)
            {
                _Logger.LogError($"Error al guardar los precios fijos \nError: {exception.Message} \nStack: {exception.StackTrace}");
            }
        }

        public void UpdateFixedPrices(FixedPrice[] fixedPrices)
        {
            try
            {
                _Context.FixedPrices.UpdateRange(fixedPrices);
                _Context.SaveChanges();
            }
            catch (Exception exception)
            {
                _Logger.LogError($"Error al actualizar los precios fijos \nError: {exception.Message} \nStack: {exception.StackTrace}");
            }
        }
    }
}