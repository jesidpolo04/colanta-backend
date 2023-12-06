namespace colanta_backend.App.Taxes
{
    using System.Collections.Generic;
    using System.Linq;
    using colanta_backend.App.Taxes.Services;

    public class CalculateOrderTaxes
    {
        TaxService _TaxService;

        public CalculateOrderTaxes(TaxService TaxService)
        {
            _TaxService = TaxService;
        }

        public List<TaxesResponse> Execute(VtexCalculateOrderTaxesRequest request)
        {
            var taxesResponses = new List<TaxesResponse>();
            var productTaxesList = _TaxService.GetSiesaTaxes().Result;
            foreach (var item in request.Items)
            {
                var itemTaxes = GetProductTaxes(productTaxesList, item);
                taxesResponses.Add(new TaxesResponse{
                    Id = item.Id,
                    Taxes = itemTaxes
                });
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
                    Name = "Impuesto al consumo",
                    Description = "",
                    Value = productSiesaTaxes.ImpuestoConsumoNominal
                });
            }
            //IMPUESTO SALUDABLE NOMINAL
            if (productSiesaTaxes.ImpuestoSaludableNominal > 0)
            {
                taxList.Add(new Tax
                {
                    Name = "Impuesto saludable",
                    Description = "",
                    Value = productSiesaTaxes.ImpuestoSaludableNominal
                });
            }
            //IMPUESTO SALUDABLE PORCENTUAL
            if (productSiesaTaxes.ImpuestoSaludablePorcentual > 0)
            {
                var priceAfterDiscounts = item.ItemPrice + item.DiscountPrice;
                taxList.Add(new Tax
                {
                    Name = "Impuesto saludable",
                    Description = "",
                    Value = priceAfterDiscounts * (productSiesaTaxes.ImpuestoSaludablePorcentual / 100)
                });
            }
            //IVA
            if (productSiesaTaxes.Iva > 0)
            {
                var priceAfterDiscounts = item.ItemPrice + item.DiscountPrice;
                taxList.Add(new Tax
                {
                    Name = "Iva",
                    Description = "",
                    Value = priceAfterDiscounts * (productSiesaTaxes.Iva / 100)
                });
            }
            return taxList;
        }
    }
}