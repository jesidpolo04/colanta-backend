namespace colanta_backend.App.Orders.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using Products.Domain;
    using Orders.Domain;
    using Shared.Domain;
    using Promotions.Domain;
    using System.Threading.Tasks;
    using GiftCards.Domain;
    using colanta_backend.App.Taxes;
    using colanta_backend.App.Taxes.Services;

    public class VtexOrderToSiesaOrderMapper
    {
        private SkusRepository skusLocalRepository;
        private PromotionsRepository promotionsLocalRepository;
        private WrongAddressesRepository wrongAddressesRepository;
        private PoundSkusService poundSkusService;
        private TaxService taxService;
        public VtexOrderToSiesaOrderMapper(
            SkusRepository skusLocalRepository,
            PromotionsRepository promotionsLocalRepository,
            WrongAddressesRepository wrongAddressesRepository,
            TaxService taxService
        )
        {
            this.skusLocalRepository = skusLocalRepository;
            this.promotionsLocalRepository = promotionsLocalRepository;
            this.wrongAddressesRepository = wrongAddressesRepository;
            this.poundSkusService = new PoundSkusService(skusLocalRepository);
            this.taxService = taxService;
        }
        public async Task<SiesaOrderDto> getSiesaOrderDto(VtexOrderDto vtexOrder)
        {
            var shippingData = vtexOrder.shippingData;
            var paymentData = vtexOrder.paymentData;
            var addressCorrector = new AddressCorrector(wrongAddressesRepository);
            SiesaOrderDto siesaOrder = new SiesaOrderDto();
            //Header
            siesaOrder.Encabezado.C263CO = this.getOperationCenter(shippingData);
            siesaOrder.Encabezado.C263Fecha = this.getDate(vtexOrder);
            siesaOrder.Encabezado.C263DocTercero = vtexOrder.clientProfileData.document;
            siesaOrder.Encabezado.C263FechaEntrega = this.getEstimateDeliveryDate(shippingData);
            siesaOrder.Encabezado.C263ReferenciaVTEX = vtexOrder.orderId;
            siesaOrder.Encabezado.C263CondPago = this.getPaymentCondition(paymentData);
            siesaOrder.Encabezado.C263ReferenciaPago = this.getHeaderPaymentReference(paymentData);
            siesaOrder.Encabezado.C263PagoContraentrega = this.isUponDelivery(paymentData);
            siesaOrder.Encabezado.C263ValorEnvio = this.getTotal(vtexOrder.totals, "Shipping");
            siesaOrder.Encabezado.C263Notas = this.getObservation(vtexOrder);
            siesaOrder.Encabezado.C263Direccion = this.getSiesaAddressFromVtexAdress(vtexOrder.shippingData.address);
            siesaOrder.Encabezado.C263Nombres = vtexOrder.shippingData.address.receiverName;
            siesaOrder.Encabezado.C263Departamento = addressCorrector.correctStateIfIsWrong(vtexOrder.shippingData.address.country, vtexOrder.shippingData.address.state, vtexOrder.shippingData.address.city);
            siesaOrder.Encabezado.C263Ciudad = addressCorrector.correctCityIfIsWrong(vtexOrder.shippingData.address.country, vtexOrder.shippingData.address.state, vtexOrder.shippingData.address.city);
            siesaOrder.Encabezado.C263Negocio = this.getBusinessFromSalesChannel(vtexOrder.salesChannel);
            siesaOrder.Encabezado.C263TotalPedido = vtexOrder.value / 100;
            siesaOrder.Encabezado.C263TotalDescuentos = this.getTotal(vtexOrder.totals, "Discounts");
            siesaOrder.Encabezado.C263RecogeEnTienda = this.pickupInStore(shippingData);
            siesaOrder.Encabezado.C263FechaRecoge = siesaOrder.Encabezado.C263RecogeEnTienda ? this.pickupDateTime(shippingData) : null;
            siesaOrder.Encabezado.C263Telefono = vtexOrder.clientProfileData.phone;


            foreach (Transaction transaction in vtexOrder.paymentData.transactions)
            {
                foreach (Payment payment in transaction.payments)
                {
                    var wayToPay = new WayToPayDto
                    {
                        C263FormaPago = this.getWayToPay(payment),
                        C263ReferenciaPago = this.getTransactionReference(payment),
                        C263Valor = payment.value / 100
                    };
                    siesaOrder.FormasPago.Add(wayToPay);
                }
            }

            int itemConsecutive = 1;
            foreach (Item vtexItem in vtexOrder.items)
            {
                string refId = getItemSiesaRef(vtexItem.refId).Result;
                SiesaOrderDetailDto siesaDetail = new SiesaOrderDetailDto
                {
                    C263DetCO = siesaOrder.Encabezado.C263CO,
                    C263NroDetalle = itemConsecutive,
                    C263ReferenciaItem = refId,
                    C263VariacionItem = this.getItemVariationSiesaRef(vtexItem.refId),
                    C263IndObsequio = vtexItem.isGift ? 1 : 0,
                    C263UnidMedida = this.getMeasurementUnit(vtexItem),
                    C263Cantidad = this.quantity(vtexItem),
                    C263Precio = this.price(vtexItem),
                    C263Notas = "sin notas",
                    C263Impuesto = 0,
                    C263ReferenciaVTEX = siesaOrder.Encabezado.C263ReferenciaVTEX
                };
                siesaOrder.Detalles.Add(siesaDetail);

                int discountConsecutive = 1;
                foreach (PriceTag vtexDiscount in vtexItem.priceTags)
                {
                    if (siesaDetail.C263IndObsequio == 1) continue;
                    if (vtexDiscount.identifier == null) continue;
                    SiesaOrderDiscountDto siesaDiscount = new SiesaOrderDiscountDto
                    {
                        C263DestoCO = siesaOrder.Encabezado.C263CO,
                        C263ReferenciaDescuento = await this.getPromotionSiesaRef(vtexDiscount.identifier),
                        C263ReferenciaVTEX = siesaOrder.Encabezado.C263ReferenciaVTEX,
                        C263NroDetalle = itemConsecutive,
                        C263OrdenDescto = discountConsecutive,
                        C263Tasa = 0,
                        C263Valor = (vtexDiscount.value / 100) * (-1)
                    };
                    siesaOrder.Descuentos.Add(siesaDiscount);
                    discountConsecutive++;
                }

                SiesaOrderTaxDto taxes = new SiesaOrderTaxDto
                {
                    C263CodPedido = 0,
                    C263ReferenciaItem = refId,
                    C263NroDetalle = itemConsecutive,
                    C263PrecioBase = price(vtexItem),
                    C263PrecioCompleto = totalPricePerItem(vtexItem),
                    C263IpoConsumoValor = 0,
                    C263SaludablePorcen = 0,
                    C263SaludablePorcenValor = 0, //por petición de Cristian Ramirez queda en 0
                    C263SaludableValor = 0,
                    C263IvaPorcen = 0,
                    C263IvaValor = 0 // por petición de Cristian Ramirez queda en 0
                };
                int totalTaxes = 0;

                foreach (PriceTag priceTag in vtexItem.priceTags)
                {
                    var taxesList = taxService.GetSiesaTaxes().Result;
                    var productTaxes = taxService.FindProductTaxes(taxesList, refId);
                    string priceTagName = priceTag.name;
                    string[] priceTagNameWords = priceTagName.Split("@");
                    if (!(priceTagNameWords.Length > 1) && priceTagNameWords[0] != "TAXHUB") continue;
                    totalTaxes++;
                    string taxName = priceTagNameWords[1];

                    if (taxName.Equals(TaxesNames.IVA))
                    {
                        taxes.C263IvaPorcen = productTaxes.Iva;
                    }
                    else if (taxName.Equals(TaxesNames.IMPUESTO_AL_CONSUMO))
                    {
                        taxes.C263IpoConsumoValor = productTaxes.ImpuestoConsumoNominal;
                    }
                    else if (taxName.Equals(TaxesNames.IMPUESTO_SALUDABLE))
                    {
                        taxes.C263SaludablePorcen = productTaxes.ImpuestoSaludablePorcentual;
                        taxes.C263SaludableValor = productTaxes.ImpuestoSaludableNominal;
                    }
                }

                if (totalTaxes > 0) siesaOrder.ImpuestosPedido.Add(taxes);
                itemConsecutive++;
            }
            return siesaOrder;
        }

        private decimal quantity(Item item)
        {
            string siesaId = item.refId.Split("_")[2];
            if (this.poundSkusService.isPoundSku(siesaId))
            {
                return item.quantity * item.unitMultiplier.Value / 2;
            }
            return item.quantity * item.unitMultiplier.Value;
        }

        private decimal price(Item item)
        {
            string siesaId = item.refId.Split("_")[2];
            if (this.poundSkusService.isPoundSku(siesaId))
            {
                return item.price / 100 * 2;
            }
            return item.price / 100;
        }

        private decimal totalPricePerItem(Item item)
        {
            decimal totalTaxesValue = 0;
            foreach (PriceTag priceTag in item.priceTags)
            {
                string priceTagName = priceTag.name;
                string[] priceTagNameWords = priceTagName.Split("@");
                if (!(priceTagNameWords.Length > 1) && priceTagNameWords[0] != "TAXHUB") continue;
                totalTaxesValue += priceTag.rawValue / item.quantity;
            }
            return (item.sellingPrice / 100) + totalTaxesValue;
        }

        private bool isUponDelivery(PaymentData paymentData)
        {
            var payments = paymentData.transactions[0].payments;
            foreach (Payment payment in payments)
            {
                if (
                    payment.paymentSystem == PaymentMethods.CARD_PROMISSORY.id ||
                    payment.paymentSystem == PaymentMethods.CONTRAENTREGA.id ||
                    payment.paymentSystem == PaymentMethods.EFECTIVO.id
                    ) return true;
            }
            return false;
        }

        private string getSiesaAddressFromVtexAdress(Address vtexAddress)
        {
            string address = vtexAddress.street;
            if (vtexAddress.number != null && vtexAddress.number != "")
                address += $" número: {vtexAddress.number}";
            if (vtexAddress.complement != null && vtexAddress.complement != "") address += $" complemento: {vtexAddress.complement}";
            address += $" barrio: {vtexAddress.neighborhood}";
            if (vtexAddress.reference != null) address = $" {vtexAddress.reference}";
            return address;
        }

        private string getBusinessFromSalesChannel(string salesChannelId)
        {
            if (salesChannelId == "1")
            {
                return "mercolanta";
            }
            else
            {
                return "agrocolanta";
            }
        }

        private decimal getTotal(List<Total> totals, string totalId)
        {
            //values can be: "Items" , "Discounts" , "Shipping"
            decimal value = 0;
            foreach (Total total in totals)
            {
                if (total.id == totalId)
                {
                    value = total.value / 100;
                }
                if (value < 0)
                {
                    value = value * (-1);
                }
            }
            return value;
        }

        private async Task<string> getItemSiesaRef(string concatSiesaId)
        {
            Sku sku = await this.skusLocalRepository.getSkuByConcatSiesaId(concatSiesaId);
            if (sku != null) return sku.ref_id;
            else return "";
        }

        private string getItemVariationSiesaRef(string concatSiesaId)
        {
            Sku sku = this.skusLocalRepository.getSkuByConcatSiesaId(concatSiesaId).Result;
            if (sku == null) return "";
            else if (sku.siesa_id == sku.ref_id) return "";
            else return sku.siesa_id;
        }

        private async Task<string> getPromotionSiesaRef(string vtexId)
        {
            Promotion promotion = await this.promotionsLocalRepository.getPromotionByVtexId(vtexId);
            if (promotion != null)
            {
                return promotion.siesa_id;
            }
            else
            {
                return "";
            }
        }

        private string getHeaderPaymentReference(PaymentData paymentData)
        {
            var payments = paymentData.transactions[0].payments;
            //recorre en busca de pago con cupo
            foreach (Payment payment in payments)
            {
                if (PaymentMethods.CUSTOMER_CREDIT.id == payment.paymentSystem) return payment.tid;
                if (PaymentMethods.GIFTCARD.id == payment.paymentSystem && payment.giftCardProvider == "cupo") return payment.giftCardId;
            }
            //ahora recorre en busca de pagos distintos de giftcard
            foreach (Payment payment in payments)
            {
                if (PaymentMethods.WOMPI.id == payment.paymentSystem) return payment.tid;
                if (PaymentMethods.CREDIBANCO.id == payment.paymentSystem) return payment.tid;
                if (PaymentMethods.EFECTIVO.id == payment.paymentSystem) return payment.tid;
                if (PaymentMethods.CONTRAENTREGA.id == payment.paymentSystem) return payment.tid;
            }
            //devuelve el id de la giftcard como ultimo recurso
            return payments[0].giftCardId;
        }

        private bool pickupInStore(ShippingData shippingData)
        {
            if (shippingData.address.addressType == "pickup") return true;
            return false;
        }

        private string? pickupDateTime(ShippingData shippingData)
        {
            //Se deben agregar 5 horas a la hora de VTEX ya que dice estar en formato UTC pero enrealidad viene con la hora Colombia (UTC-5)
            var estimatedDate = shippingData.logisticsInfo[0].shippingEstimateDate;
            return estimatedDate.HasValue ? estimatedDate.Value.AddHours(5).ToString(DateFormats.FECHA_RECOGE) : null;
        }

        private string getEstimateDeliveryDate(ShippingData shippingData)
        {
            int defaultHours = 2;
            if (!shippingData.logisticsInfo[0].shippingEstimateDate.HasValue) return DateTime.Now.AddHours(defaultHours).ToString(DateFormats.UTC);
            return shippingData.logisticsInfo[0].shippingEstimateDate.Value.ToString(DateFormats.UTC);
        }

        private string getOperationCenter(ShippingData shippingData)
        {
            if (shippingData.address.addressType == "pickup") return shippingData.address.addressId;
            else return shippingData.logisticsInfo[0].polygonName;
        }

        private string getPaymentCondition(PaymentData paymentData)
        {
            foreach (Payment payment in paymentData.transactions[0].payments)
            {
                if (PaymentMethods.CUSTOMER_CREDIT.id == payment.paymentSystem) return "CUPO";
                if (PaymentMethods.GIFTCARD.id == payment.paymentSystem && payment.giftCardProvider == Providers.CUPO) return "CUPO";
            }
            return "CON";
        }

        private string getWayToPay(Payment payment)
        {
            if (PaymentMethods.GIFTCARD.id == payment.paymentSystem && Providers.GIFTCARDS == payment.giftCardProvider) return "GIFTCARD";
            if (PaymentMethods.GIFTCARD.id == payment.paymentSystem && Providers.CUPO == payment.giftCardProvider) return "CUPO";
            if (PaymentMethods.CONTRAENTREGA.id == payment.paymentSystem && PaymentMethods.CONTRAENTREGA.name == payment.paymentSystemName) return "CONTRAENTREGA";
            if (PaymentMethods.WOMPI.id == payment.paymentSystem) return "WOMPI";
            if (PaymentMethods.CREDIBANCO.id == payment.paymentSystem) return "CREDIBANCO";
            if (PaymentMethods.EFECTIVO.id == payment.paymentSystem && PaymentMethods.EFECTIVO.name == payment.paymentSystemName) return "EFECTIVO";
            if (PaymentMethods.CARD_PROMISSORY.id == payment.paymentSystem) return "CARD_PROMISSORY";
            if (PaymentMethods.CUSTOMER_CREDIT.id == payment.paymentSystem) return "CUPO";
            else return "OTRO";
        }

        private string getTransactionReference(Payment payment)
        {
            if (PaymentMethods.CONTRAENTREGA.id == payment.paymentSystem) return payment.tid != null ? payment.tid : "";
            if (PaymentMethods.EFECTIVO.id == payment.paymentSystem) return payment.tid;
            if (PaymentMethods.CARD_PROMISSORY.id == payment.paymentSystem) return payment.tid != null ? payment.tid : "";
            if (PaymentMethods.CUSTOMER_CREDIT.id == payment.paymentSystem) return payment.tid;
            if (PaymentMethods.GIFTCARD.id == payment.paymentSystem) return payment.giftCardId;
            if (PaymentMethods.WOMPI.id == payment.paymentSystem) return payment.tid;
            if (PaymentMethods.CREDIBANCO.id == payment.paymentSystem) return payment.tid;
            else return payment.tid;
        }

        private string getObservation(VtexOrderDto vtexOrder)
        {
            return vtexOrder.openTextField != null ? vtexOrder.openTextField.value : "sin observaciones";
        }

        private string getDate(VtexOrderDto vtexOrder)
        {
            var now = DateTime.Now;
            if (DateTime.Compare(vtexOrder.creationDate, now) < 0)
            {
                return now.ToString(DateFormats.UTC);
            }
            else
            {
                return vtexOrder.creationDate.ToString(DateFormats.UTC);
            }
        }

        private string getMeasurementUnit(Item item)
        {
            var skuSiesaId = item.refId.Split("_")[2];
            if (item.measurementUnit == "kg") return "KG";
            else if (item.measurementUnit == "lb")
            {
                if (this.poundSkusService.isPoundSku(skuSiesaId))
                {
                    return "KG";
                }
                else
                {
                    return "LB";
                }
            }
            else return "UND";
        }
    }
}
