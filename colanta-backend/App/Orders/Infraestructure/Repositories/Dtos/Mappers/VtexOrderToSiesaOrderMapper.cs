namespace colanta_backend.App.Orders.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using Products.Domain;
    using Orders.Domain;
    using Shared.Domain;
    using Promotions.Domain;
    using SiesaOrders.Domain;
    using System.Threading.Tasks;
    using GiftCards.Domain;
    public class VtexOrderToSiesaOrderMapper
    {
        private SkusRepository skusLocalRepository;
        private PromotionsRepository promotionsLocalRepository;
        private WrongAddressesRepository wrongAddressesRepository;
        public VtexOrderToSiesaOrderMapper(SkusRepository skusLocalRepository, PromotionsRepository promotionsLocalRepository, WrongAddressesRepository wrongAddressesRepository)
        {
            this.skusLocalRepository = skusLocalRepository;
            this.promotionsLocalRepository = promotionsLocalRepository;
            this.wrongAddressesRepository = wrongAddressesRepository;

        }
        public async Task<SiesaOrderDto> getSiesaOrderDto(VtexOrderDto vtexOrder)
        {
            var addressCorrector = new AddressCorrector(wrongAddressesRepository);
            SiesaOrderDto siesaOrder = new SiesaOrderDto();
            //Header
            siesaOrder.Encabezado.C263CO = this.getOperationCenter(vtexOrder.shippingData.address, vtexOrder.shippingData.logisticsInfo[0]);
            siesaOrder.Encabezado.C263Fecha = this.getDate(vtexOrder);
            siesaOrder.Encabezado.C263DocTercero = vtexOrder.clientProfileData.document;
            siesaOrder.Encabezado.C263FechaEntrega = this.getEstimateDeliveryDate(vtexOrder.shippingData.logisticsInfo[0].shippingEstimateDate);
            siesaOrder.Encabezado.C263ReferenciaVTEX = vtexOrder.orderId;
            siesaOrder.Encabezado.C263CondPago = this.getPaymentCondition(vtexOrder.paymentData.transactions[0].payments);
            siesaOrder.Encabezado.C263ReferenciaPago = this.getHeaderPaymentReference(vtexOrder.paymentData.transactions[0].payments);
            siesaOrder.Encabezado.C263PagoContraentrega = this.isUponDelivery(vtexOrder.paymentData.transactions[0].payments);
            siesaOrder.Encabezado.C263ValorEnvio = this.getTotal(vtexOrder.totals, "Shipping");
            siesaOrder.Encabezado.C263Notas = this.getObservation(vtexOrder);
            siesaOrder.Encabezado.C263Direccion = this.getSiesaAddressFromVtexAdress(vtexOrder.shippingData.address);
            siesaOrder.Encabezado.C263Nombres = vtexOrder.shippingData.address.receiverName;
            siesaOrder.Encabezado.C263Departamento = addressCorrector.correctStateIfIsWrong(vtexOrder.shippingData.address.country, vtexOrder.shippingData.address.state, vtexOrder.shippingData.address.city);
            siesaOrder.Encabezado.C263Ciudad = addressCorrector.correctCityIfIsWrong(vtexOrder.shippingData.address.country, vtexOrder.shippingData.address.state, vtexOrder.shippingData.address.city);
            siesaOrder.Encabezado.C263Negocio = this.getBusinessFromSalesChannel(vtexOrder.salesChannel);
            siesaOrder.Encabezado.C263TotalPedido = vtexOrder.value / 100;
            siesaOrder.Encabezado.C263TotalDescuentos = this.getTotal(vtexOrder.totals, "Discounts");
            siesaOrder.Encabezado.C263RecogeEnTienda = this.pickupInStore(vtexOrder.shippingData.address.addressType);

            
            foreach (Transaction transaction in vtexOrder.paymentData.transactions)
            {
                foreach(Payment payment in transaction.payments)
                {
                    var wayToPay = new WayToPayDto();
                    wayToPay.C263FormaPago = this.getWayToPay(payment);
                    wayToPay.C263ReferenciaPago = this.getTransactionReference(payment);
                    wayToPay.C263Valor = payment.value / 100;
                    siesaOrder.FormasPago.Add(wayToPay);
                }
            }
            
            int itemConsecutive = 0;
            foreach (Item vtexItem in vtexOrder.items)
            {
                itemConsecutive++;
                SiesaOrderDetailDto siesaDetail = new SiesaOrderDetailDto();
                siesaDetail.C263DetCO = siesaOrder.Encabezado.C263CO;
                siesaDetail.C263NroDetalle = itemConsecutive;
                siesaDetail.C263ReferenciaItem = await this.getItemSiesaRef(vtexItem.refId);
                siesaDetail.C263VariacionItem = this.getItemVariationSiesaRef(vtexItem.refId);
                siesaDetail.C263IndObsequio = vtexItem.isGift ? 1 : 0;
                siesaDetail.C263UnidMedida = this.getMeasurementUnit(vtexItem.measurementUnit);
                siesaDetail.C263Cantidad = vtexItem.quantity * ((decimal) vtexItem.unitMultiplier);
                siesaDetail.C263Precio = vtexItem.price / 100;
                siesaDetail.C263Notas = "sin notas";
                siesaDetail.C263Impuesto = 0;
                siesaDetail.C263ReferenciaVTEX = siesaOrder.Encabezado.C263ReferenciaVTEX;
                siesaOrder.Detalles.Add(siesaDetail);

                int discountConsecutive = 0;
                foreach(PriceTag vtexDiscount in vtexItem.priceTags)
                {
                    if (siesaDetail.C263IndObsequio == 1) continue;
                    if (vtexDiscount.identifier == null) continue;
                    discountConsecutive++;
                    SiesaOrderDiscountDto siesaDiscount = new SiesaOrderDiscountDto();
                    siesaDiscount.C263DestoCO = siesaOrder.Encabezado.C263CO;
                    siesaDiscount.C263ReferenciaDescuento = await this.getPromotionSiesaRef(vtexDiscount.identifier);
                    siesaDiscount.C263ReferenciaVTEX = siesaOrder.Encabezado.C263ReferenciaVTEX;
                    siesaDiscount.C263NroDetalle = itemConsecutive;
                    siesaDiscount.C263OrdenDescto = discountConsecutive;
                    siesaDiscount.C263Tasa = 0;
                    siesaDiscount.C263Valor = (vtexDiscount.value / 100) * (-1);
                    siesaOrder.Descuentos.Add(siesaDiscount);
                }
            }
            return siesaOrder;
        }

        private bool isUponDelivery(List<Payment> payments)
        {
            foreach(Payment payment in payments)
            {
                if(
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
            if (vtexAddress.complement != null && vtexAddress.complement != "") address += $" complemento: {vtexAddress.complement}";
            address += $" Barrio: {vtexAddress.neighborhood}";
            if (vtexAddress.reference != null) address = $" {vtexAddress.reference}";
            return address;
        }

        private string getBusinessFromSalesChannel(string salesChannelId)
        {
            if(salesChannelId == "1")
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
            foreach(Total total in totals)
            {
                if(total.id == totalId)
                {
                    value = total.value / 100;
                }
                if(value < 0)
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
            if(promotion != null)
            {
                return promotion.siesa_id;
            }
            else
            {
                return "";
            }
        }

        private string getHeaderPaymentReference(List<Payment> payments)
        {
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
                if (PaymentMethods.EFECTIVO.id == payment.paymentSystem) return payment.tid;
                if (PaymentMethods.CONTRAENTREGA.id == payment.paymentSystem) return payment.tid;
            }
            //devuelve el id de la giftcard como ultimo recurso
            return payments[0].giftCardId;
        }

        private bool pickupInStore(string addressType)
        { 
            if (addressType == "pickup") return true;
            return false;
        }

        private string getEstimateDeliveryDate(DateTime? date)
        {
            int defaultHours = 2;
            if (date == null) return DateTime.Now.AddHours(defaultHours).ToString(DateFormats.UTC);
            DateTime notNullDate = (DateTime)date;
            return notNullDate.ToString(DateFormats.UTC);
        }

        private string getOperationCenter(Address address, LogisticsInfo logisticsInfo)
        {
            if (address.addressType == "pickup") return address.addressId;
            else return logisticsInfo.polygonName;
        }

        private string getPaymentCondition(List<Payment> payments)
        {
            foreach (Payment payment in payments)
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
            else return payment.tid;
        }

        private string getObservation(VtexOrderDto vtexOrder)
        {
            return vtexOrder.openTextField != null ? vtexOrder.openTextField.value : "sin observaciones"; 
        }

        private string getDate(VtexOrderDto vtexOrder)
        {
            var now = DateTime.Now;
            if(DateTime.Compare(vtexOrder.creationDate, now) < 0)
            {
                return now.ToString(DateFormats.UTC);
            }
            else
            {
                return vtexOrder.creationDate.ToString(DateFormats.UTC);
            }
        }

        private string getMeasurementUnit(string vtexMeasurementUnit)
        {
            if (vtexMeasurementUnit == "kg") return "KG";
            else if (vtexMeasurementUnit == "lb") return "LB";
            else return "UND";
        }
    }
}
