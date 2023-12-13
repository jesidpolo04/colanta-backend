using colanta_backend.App.Orders.SiesaOrders.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.Orders.SiesaOrders.Infraestructure
{
    using Shared.Infraestructure;
    using Microsoft.Extensions.Configuration;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

    public class SiesaOrdersEFRepository : SiesaOrdersRepository
    {
        private ColantaContext dbContext;
        public SiesaOrdersEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }
        public async Task<SiesaOrderDetail> deleteSiesaOrderDetail(SiesaOrderDetail siesaOrderDetail)
        {
            EFSiesaOrderDetail efSiesaOrderDetail = this.dbContext.SiesaOrderDetails.Find(siesaOrderDetail.id);
            this.dbContext.Remove(efSiesaOrderDetail);
            this.dbContext.SaveChanges();
            return siesaOrderDetail;
        }

        public async Task<SiesaOrderDiscount> deleteSiesaOrderDetail(SiesaOrderDiscount siesaOrderDiscount)
        {
            EFSiesaOrderDiscount efSiesaOrderDiscount = this.dbContext.SiesaOrderDiscounts.Find(siesaOrderDiscount);
            this.dbContext.Remove(efSiesaOrderDiscount);
            this.dbContext.SaveChanges();
            return siesaOrderDiscount;
        }

        public async Task<SiesaOrder[]> getAllSiesaOrdersByFinalizado(bool finalizado)
        {
            EFSiesaOrder[] efSiesaOrders = this.dbContext.SiesaOrders
                .Include(siesaOrder => siesaOrder.detalles)
                .Include(siesaOrder => siesaOrder.descuentos)
                .Include(siesaOrder => siesaOrder.impuestos)
                .Where(siesaOrder => siesaOrder.finalizado == finalizado).ToArray();
            List<SiesaOrder> siesaOrders = new List<SiesaOrder>();
            foreach (EFSiesaOrder efSiesaOrder in efSiesaOrders)
            {
                siesaOrders.Add(efSiesaOrder.getSiesaOrderFromEfSiesaOrder());
            }
            return siesaOrders.ToArray();
        }

        public async Task<SiesaOrder> getSiesaOrderBySiesaId(string siesaId)
        {
            EFSiesaOrder[] siesaOrders = this.dbContext.SiesaOrders
                .Include(siesaOrder => siesaOrder.detalles)
                .Include(siesaOrder => siesaOrder.descuentos)
                .Include(siesaOrder => siesaOrder.impuestos)
                .Where(siesaOrder => siesaOrder.siesa_id == siesaId).ToArray();
            if (siesaOrders.Length > 0)
            {
                return siesaOrders.First().getSiesaOrderFromEfSiesaOrder();
            }
            return null;
        }

        public async Task<SiesaOrder> getSiesaOrderByVtexId(string vtexId)
        {
            EFSiesaOrder[] siesaOrders = this.dbContext.SiesaOrders
                .Include(siesaOrder => siesaOrder.detalles)
                .Include(siesaOrder => siesaOrder.descuentos)
                .Include(siesaOrder => siesaOrder.impuestos)
                .Where(siesaOrder => siesaOrder.referencia_vtex == vtexId).ToArray();
            if (siesaOrders.Length > 0)
            {
                return siesaOrders.First().getSiesaOrderFromEfSiesaOrder();
            }
            return null;
        }

        public async Task<SiesaOrder[]> getSiesaOrdersByDocument(string document)
        {
            EFSiesaOrder[] efSiesaOrders = this.dbContext.SiesaOrders
                .Include(siesaOrders => siesaOrders.detalles)
                .Include(siesaOrders => siesaOrders.descuentos)
                .Include(siesaOrder => siesaOrder.impuestos)
                .Where(siesaOrder => siesaOrder.doc_tercero == document).ToArray();
            List<SiesaOrder> siesaOrders = new List<SiesaOrder>();
            foreach (EFSiesaOrder efSiesaOrder in efSiesaOrders)
            {
                siesaOrders.Add(efSiesaOrder.getSiesaOrderFromEfSiesaOrder());
            }
            return siesaOrders.ToArray();
        }

        public async Task<SiesaOrder> saveSiesaOrder(SiesaOrder siesaOrder)
        {
            EFSiesaOrder efSiesaOrder = new EFSiesaOrder();
            efSiesaOrder.setEfSiesaOrderFromSiesaOrder(siesaOrder);
            this.dbContext.Add(efSiesaOrder);
            this.dbContext.SaveChanges();
            return await this.getSiesaOrderByVtexId(siesaOrder.referencia_vtex);
        }

        public async Task<SiesaOrder> updateSiesaOrder(SiesaOrder siesaOrder)
        {
            EFSiesaOrder efSiesaOrder = this.dbContext.SiesaOrders
                .Include(efSiesaOrder => efSiesaOrder.detalles)
                .Include(efSiesaOrder => efSiesaOrder.descuentos)
                .Include(siesaOrder => siesaOrder.impuestos)
                .Where(efSiesaOrder => efSiesaOrder.referencia_vtex == siesaOrder.referencia_vtex).First();
            efSiesaOrder.setEfSiesaOrderFromSiesaOrder(siesaOrder);
            this.dbContext.SaveChanges();
            return efSiesaOrder.getSiesaOrderFromEfSiesaOrder();
        }

        public async Task<SiesaOrderDetail> updateSiesaOrderDetail(SiesaOrderDetail siesaOrderDetail)
        {
            EFSiesaOrderDetail efSiesaOrderDetail = this.dbContext.SiesaOrderDetails.Find(siesaOrderDetail.id);
            efSiesaOrderDetail.setEfSiesaOrderDetailFromSiesaOrderDetail(siesaOrderDetail);
            this.dbContext.SaveChanges();
            return siesaOrderDetail;
        }

        public async Task<SiesaOrderDiscount> updateSiesaOrderDiscount(SiesaOrderDiscount siesaOrderDiscount)
        {
            EFSiesaOrderDiscount efSiesaOrderDiscount = this.dbContext.SiesaOrderDiscounts.Find(siesaOrderDiscount.id);
            efSiesaOrderDiscount.setEfSiesaOrderDiscountFromSiesaOrderDiscount(siesaOrderDiscount);
            this.dbContext.SaveChanges();
            return siesaOrderDiscount;
        }

        public async Task<SiesaOrderTax> updateSiesaOrderTax(SiesaOrderTax siesaOrderTax)
        {
            EFSiesaOrderTax eFSiesaOrderTax = dbContext.SiesaOrderTaxes.Find(siesaOrderTax.Id);
            eFSiesaOrderTax.SetEfSiesaOrderTaxFromSiesaOrderTax(siesaOrderTax);
            dbContext.SaveChanges();
            return siesaOrderTax;
        }
    }
}
