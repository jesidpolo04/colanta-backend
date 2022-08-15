namespace colanta_backend.App.Orders.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using Products.Domain;
    using Orders.Domain;
    using Promotions.Domain;
    using SiesaOrders.Domain;
    using System.Threading.Tasks;
    public class VtexOrderToSiesaOrderMapper
    {
        private SkusRepository skusLocalRepository;
        private PromotionsRepository promotionsLocalRepository;
        public VtexOrderToSiesaOrderMapper(SkusRepository skusLocalRepository, PromotionsRepository promotionsLocalRepository)
        {
            this.skusLocalRepository = skusLocalRepository;
            this.promotionsLocalRepository = promotionsLocalRepository;
        }
        public async Task<SiesaOrderDto> getSiesaOrderDto(VtexOrderDto vtexOrder)
        {
            SiesaOrderDto siesaOrder = new SiesaOrderDto();
            SiesaOrderHeaderDto header = new SiesaOrderHeaderDto();
            siesaOrder.Encabezado = header;
            //Header
            siesaOrder.Encabezado.C263CO = this.getOperationCenter(vtexOrder.shippingData.address);
            siesaOrder.Encabezado.C263Fecha = vtexOrder.creationDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            siesaOrder.Encabezado.C263DocTercero = vtexOrder.clientProfileData.document;
            siesaOrder.Encabezado.C263FechaEntrega = this.getEstimateDeliveryDate(vtexOrder.shippingData.logisticsInfo[0].shippingEstimateDate);
            siesaOrder.Encabezado.C263ReferenciaVTEX = vtexOrder.orderId;
            siesaOrder.Encabezado.C263CondPago = this.getPaymentCondition(vtexOrder.paymentData.transactions[0].payments[0], vtexOrder.clientProfileData);
            siesaOrder.Encabezado.C263ReferenciaPago = vtexOrder.paymentData.transactions[0].payments[0].tid;
            siesaOrder.Encabezado.C263ValorEnvio = this.getTotal(vtexOrder.totals, "Shipping");
            siesaOrder.Encabezado.C263Notas = "sin observaciones";
            siesaOrder.Encabezado.C263Direccion = this.getSiesaAddressFromVtexAdress(vtexOrder.shippingData.selectedAddresses[0]);
            siesaOrder.Encabezado.C263Departamento = vtexOrder.shippingData.address.state;
            siesaOrder.Encabezado.C263Ciudad = vtexOrder.shippingData.address.city;
            siesaOrder.Encabezado.C263Negocio = this.getBusinessFromSalesChannel(vtexOrder.salesChannel);
            siesaOrder.Encabezado.C263TotalPedido = vtexOrder.value / 100;
            siesaOrder.Encabezado.C263TotalDescuentos = this.getTotal(vtexOrder.totals, "Discounts");
            siesaOrder.Encabezado.C263RecogeEnTienda = this.pickupInStore(vtexOrder.shippingData.address.addressType);
            
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
                siesaDetail.C263UnidMedida = vtexItem.measurementUnit == "un" ? "UND" : vtexItem.measurementUnit;
                siesaDetail.C263Cantidad = vtexItem.quantity;
                siesaDetail.C263Precio = vtexItem.price / 100;
                siesaDetail.C263Notas = "sin notas";
                siesaDetail.C263Impuesto = 0;
                siesaDetail.C263ReferenciaVTEX = siesaOrder.Encabezado.C263ReferenciaVTEX;
                siesaOrder.Detalles.Add(siesaDetail);

                int discountConsecutive = 0;
                foreach(PriceTag vtexDiscount in vtexItem.priceTags)
                {
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

        private string getSiesaAddressFromVtexAdress(SelectedAddress vtexAddress)
        {
            string address = "Calle " + vtexAddress.street + " " + vtexAddress.complement + " - ";
            address += "Barrio: " + vtexAddress.neighborhood + " ";
            address += vtexAddress.reference;
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

        private bool pickupInStore(string addressType)
        { 
            if (addressType == "pickup") return true;
            return false;
        }

        private string getEstimateDeliveryDate(DateTime? date)
        {
            int defaultHours = 2;
            if (date == null) return DateTime.Now.AddHours(defaultHours).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            DateTime notNullDate = (DateTime)date;
            return notNullDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }

        private string getOperationCenter(Address address)
        {
            if (address.addressType == "pickup") return address.addressId;
            else return "Por Definir";
        }

        private string getPaymentCondition(Payment payment, ClientProfileData client)
        {
            if (PaymentMethods.CONTRAENTREGA.id == payment.paymentSystem) return "CON";
            if (PaymentMethods.EFECTIVO.id == payment.paymentSystem) return "CON";
            if (PaymentMethods.CARD_PROMISSORY.id == payment.paymentSystem) return "CARD_PROMISSORY";
            if (PaymentMethods.CUSTOMER_CREDIT.id == payment.paymentSystem) return "CUPO";
            if (PaymentMethods.WOMPI.id == payment.paymentSystem) return "WOMPI";
            else return "OTRO";
        }
    }
}
