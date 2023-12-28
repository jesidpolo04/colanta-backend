using colanta_backend.App.Prices.Domain;
using colanta_backend.App.Taxes;
using colanta_backend.App.Taxes.Services;

namespace colanta_backend.App.Promotions{
    public class PromotionPriceCalculator{
        private TaxService _TaxService;
        private ProductSiesaTaxes[] _ProductSiesaTaxes;
        public PromotionPriceCalculator(TaxService taxService){
            _TaxService = taxService;
            _ProductSiesaTaxes = _TaxService.GetSiesaTaxes().Result;
        }

        public decimal CalculatePrice(Price price, decimal discountPercentage){
            var taxes = _TaxService.FindProductTaxes(_ProductSiesaTaxes, price.sku.siesa_id);
            if(taxes != null){
                var discountBasePrice = price.base_price - (price.base_price * (discountPercentage / 100));
                var iva = discountBasePrice * (taxes.Iva / 100);
                var impuestoSaludableNominal = taxes.ImpuestoSaludableNominal;
                var impuestoSaludablePorcentual = discountBasePrice * (taxes.ImpuestoSaludablePorcentual / 100);
                var impuestoAlConsumo = taxes.ImpuestoConsumoNominal;
                return discountBasePrice + iva + impuestoSaludablePorcentual + impuestoSaludableNominal + impuestoAlConsumo;
            }
            return price.price - (price.price * (discountPercentage / 100));
        }
    }
}