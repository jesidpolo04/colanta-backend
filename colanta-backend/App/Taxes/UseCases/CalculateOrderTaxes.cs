namespace colanta_backend.App.Taxes
{
    using System.Collections.Generic;
    using System.Linq;
    using colanta_backend.App.Taxes.Services;
    using Microsoft.Extensions.Logging;

    public class CalculateOrderTaxes
    {
        TaxService _TaxService;
        private ILogger _Logger;

        public CalculateOrderTaxes(TaxService TaxService, ILogger Logger)
        {
            _TaxService = TaxService;
            _Logger = Logger;
        }

        public List<TaxesResponse> Execute(VtexCalculateOrderTaxesRequest request)
        {
            var taxesResponses = new List<TaxesResponse>();
            var productTaxesList = _TaxService.GetSiesaTaxes().Result;
            _Logger.LogDebug(productTaxesList.ToString());
            foreach (var item in request.Items)
            {
                var itemTaxes = GetProductTaxes(productTaxesList, item);
                if (itemTaxes.Count > 0)
                {
                    taxesResponses.Add(new TaxesResponse
                    {
                        Id = item.Id,
                        Taxes = itemTaxes
                    });
                }
            }
            return taxesResponses;
        }

        private List<Tax> GetProductTaxes(ProductSiesaTaxes[] taxesList, Item item)
        {
            var taxList = new List<Tax>();
            var skuRef = item.RefId.Split("_")[1];
            var productSiesaTaxesList = taxesList.Where(productSiesaTaxes => productSiesaTaxes.IdProducto == skuRef).ToList();
            if (productSiesaTaxesList.Count < 1)
            {
                return taxList;
            }
            var productSiesaTaxes = productSiesaTaxesList.First();

            //IMPUESTO AL CONSUMO
            if (productSiesaTaxes.ImpuestoConsumoNominal > 0)
            {
                taxList.Add(new Tax
                {
                    Name = TaxesNames.IMPUESTO_AL_CONSUMO,
                    Description = "",
                    Value = productSiesaTaxes.ImpuestoConsumoNominal * item.Quantity
                });
            }
            //IMPUESTO SALUDABLE NOMINAL
            if (productSiesaTaxes.ImpuestoSaludableNominal > 0)
            {
                taxList.Add(new Tax
                {
                    Name = TaxesNames.IMPUESTO_SALUDABLE,
                    Description = "",
                    Value = productSiesaTaxes.ImpuestoSaludableNominal * item.Quantity
                });
            }
            //IMPUESTO SALUDABLE PORCENTUAL
            if (productSiesaTaxes.ImpuestoSaludablePorcentual > 0)
            {
                var priceAfterDiscounts = item.ItemPrice + item.DiscountPrice;
                taxList.Add(new Tax
                {
                    Name = TaxesNames.IMPUESTO_SALUDABLE,
                    Description = "",
                    Value = priceAfterDiscounts * (productSiesaTaxes.ImpuestoSaludablePorcentual / 100) * item.Quantity
                });
            }
            //IVA
            if (productSiesaTaxes.Iva > 0)
            {
                var priceAfterDiscounts = item.ItemPrice + item.DiscountPrice;
                taxList.Add(new Tax
                {
                    Name = TaxesNames.IVA,
                    Description = "",
                    Value = priceAfterDiscounts * (productSiesaTaxes.Iva / 100) * item.Quantity
                });
            }
            return taxList;
        }
    }
}