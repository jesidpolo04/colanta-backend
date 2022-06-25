namespace colanta_backend.App.Orders.Infraestructure
{
    using System.Collections.Generic;
    using Products.Domain;
    using Promotions.Domain;
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
        public async Task<SiesaOrderDto> getSiesaOrder(VtexOrderDto vtexOrder)
        {
            SiesaOrderDto siesaOrder = new SiesaOrderDto();
            SiesaOrderHeaderDto header = new SiesaOrderHeaderDto();
            siesaOrder.Encabezado = header;
            //Header
            siesaOrder.Encabezado.C263CO = vtexOrder.shippingData.logisticsInfo[0].deliveryIds[0].warehouseId;
            siesaOrder.Encabezado.C263Fecha = vtexOrder.creationDate;
            siesaOrder.Encabezado.C263DocTercero = vtexOrder.clientProfileData.document;
            siesaOrder.Encabezado.C263FechaEntrega = vtexOrder.shippingData.shippingEstimateDate;
            siesaOrder.Encabezado.C263ReferenciaVTEX = vtexOrder.orderId;
            siesaOrder.Encabezado.C263CondPago = vtexOrder.paymentData.transactions[0].payments[0].paymentSystem;
            siesaOrder.Encabezado.C263Notas = "sin observaciones";
            siesaOrder.Encabezado.C263Direccion = this.getSiesaAddressFromVtexAdress(vtexOrder.shippingData.selectedAddresses[0]);
            siesaOrder.Encabezado.C263Negocio = this.getBusinessFromSalesChannel(vtexOrder.salesChannel);
            siesaOrder.Encabezado.C263TotalPedido = this.getTotal(vtexOrder.totals, "Items");
            siesaOrder.Encabezado.C263TotalDescuentos = this.getTotal(vtexOrder.totals, "Discounts");
            //Details
            List<SiesaOrderDetailDto> details = new List<SiesaOrderDetailDto>();
            List<SiesaOrderDiscountDto> discounts = new List<SiesaOrderDiscountDto>();
            int consecutive = 0;

            foreach (ItemDto vtexItem in vtexOrder.items)
            {
                consecutive++;
                SiesaOrderDetailDto siesaDetail = new SiesaOrderDetailDto();
                siesaDetail.C263DetCO = vtexOrder.shippingData.logisticsInfo[0].deliveryIds[0].warehouseId;
                siesaDetail.C263NroDetalle = consecutive;
                siesaDetail.C263ReferenciaItem = await this.getSiesaSkuRefId(vtexItem.refId);
                siesaDetail.C263VariacionItem = "";
                siesaDetail.C263IndObsequio = vtexItem.isGift ? 1 : 0;
                siesaDetail.C263UnidMedida = vtexItem.measurementUnit == "un" ? "UND" : vtexItem.measurementUnit;
                siesaDetail.C263Cantidad = vtexItem.quantity;
                siesaDetail.C263Precio = vtexItem.price / 100;
                siesaDetail.C263Notas = "sin notas";
                siesaDetail.C263Impuesto = 0;
                siesaDetail.C263ReferenciaVTEX = vtexItem.id;
                details.Add(siesaDetail);

                int discountsConsecutive = 0;
                foreach(PriceTagDto vtexDiscount in vtexItem.priceTags)
                {
                    discountsConsecutive++;
                    SiesaOrderDiscountDto siesaDiscount = new SiesaOrderDiscountDto();
                    siesaDiscount.C263DestoCO = vtexOrder.shippingData.logisticsInfo[0].deliveryIds[0].warehouseId;
                    siesaDiscount.C263ReferenciaVTEX = vtexDiscount.identifier;
                    siesaDiscount.C263NroDetalle = consecutive;
                    siesaDiscount.C263OrdenDescto = discountsConsecutive;
                    siesaDiscount.C263Tasa = 0;
                    siesaDiscount.C263Valor = (vtexDiscount.value / 100) * (-1);
                }
            }
            siesaOrder.Detalles = details.ToArray();
            siesaOrder.Descuentos = discounts.ToArray();
            return siesaOrder;
        }

        private string getSiesaAddressFromVtexAdress(SelectedAddresseDto vtexAddress)
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

        private decimal getTotal(TotalDto[] totals, string totalId)
        {
            //values can be: "Items" , "Discounts" , "Shipping"
            decimal value = 0;
            foreach(TotalDto total in totals)
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

        private async Task<string> getSiesaSkuRefId(string concatSiesaId)
        {
            Sku sku = await this.skusLocalRepository.getSkuByConcatSiesaId(concatSiesaId);
            if(sku != null)
            {
                return sku.ref_id;
            }
            else
            {
                return "";
            }
        }

        private async Task<string> getSiesaPromotionId(string vtexId)
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
    }
}
