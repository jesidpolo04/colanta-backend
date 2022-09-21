namespace colanta_backend.App.Orders.Jobs
{
    using Orders.Domain;
    using Shared.Domain;
    using SiesaOrders.Domain;
    using System;
    using System.Threading.Tasks;
    using System.Text.Json;
    public class UpdateSiesaOrders
    {
        private OrdersRepository localRepository;
        private SiesaOrdersRepository siesaOrdersLocalRepository;
        private SiesaOrdersHistoryRepository siesaOrdersHistoryLocalRepository;
        private OrdersSiesaRepository siesaRepository;
        private OrdersVtexRepository vtexRepository;
        private ILogger logger;

        public UpdateSiesaOrders(
            OrdersRepository localRepository,
            SiesaOrdersRepository siesaOrdersLocalRepository,
            SiesaOrdersHistoryRepository siesaOrdersHistoryLocalRepository,
            OrdersSiesaRepository siesaRepository,
            OrdersVtexRepository vtexRepository,
            ILogger logger
            )
        {
            this.localRepository = localRepository;
            this.siesaOrdersLocalRepository = siesaOrdersLocalRepository;
            this.siesaOrdersHistoryLocalRepository = siesaOrdersHistoryLocalRepository;
            this.siesaRepository = siesaRepository;
            this.vtexRepository = vtexRepository;
            this.logger = logger;
        }

        public async Task Invoke()
        {
            try
            {
                SiesaOrder[] unfinishedSiesaOrders = await this.siesaOrdersLocalRepository.getAllSiesaOrdersByFinalizado(false);
                foreach (SiesaOrder unfinishedSiesaOrder in unfinishedSiesaOrders)
                {
                    try
                    {
                        SiesaOrder newSiesaOrder = await this.siesaRepository.getOrderBySiesaId(unfinishedSiesaOrder.siesa_id);
                        if (newSiesaOrder == null) continue;
                        if (newSiesaOrder.cancelado)
                        {
                            await this.siesaOrdersHistoryLocalRepository.saveSiesaOrderHistory(unfinishedSiesaOrder);
                            unfinishedSiesaOrder.cancelado = true;
                            await this.siesaOrdersLocalRepository.saveSiesaOrder(unfinishedSiesaOrder);
                            await this.cancelOrder(unfinishedSiesaOrder.referencia_vtex);
                            continue;
                        }
                        if (newSiesaOrder.finalizado)
                        {
                            this.updateLocalSiesaOrder(newSiesaOrder, unfinishedSiesaOrder).Wait();
                            Order order = this.localRepository.getOrderByVtexId(unfinishedSiesaOrder.referencia_vtex).Result;
                            VtexOrder vtexOrder = JsonSerializer.Deserialize<VtexOrder>(order.order_json);
                            if (vtexOrder.status == OrderVtexStates.PAYMENT_PENDING)
                            {
                                foreach(Payment payment in vtexOrder.paymentData.transactions[0].payments)
                                {
                                    if (payment.paymentSystem == PaymentMethods.CONTRAENTREGA.id ||
                                        payment.paymentSystem == PaymentMethods.EFECTIVO.id)
                                        this.approvePayment(payment.id, vtexOrder.orderId).Wait();
                                }
                            }
                        }
                    }
                    catch(SiesaException siesaException)
                    {
                        Console.WriteLine(siesaException.Message);
                        await this.logger.writelog(siesaException);
                    }
                    catch(VtexException vtexException)
                    {
                        Console.WriteLine(vtexException.Message);
                        await this.logger.writelog(vtexException);
                    }
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                await this.logger.writelog(exception);
            }
        }

        private Task updateLocalSiesaOrder(SiesaOrder newSiesaOrder, SiesaOrder unfinishedSiesaOrder)
        {
            this.siesaOrdersHistoryLocalRepository.saveSiesaOrderHistory(unfinishedSiesaOrder);
            newSiesaOrder.siesa_id = unfinishedSiesaOrder.siesa_id;
            newSiesaOrder.estado_vtex = unfinishedSiesaOrder.estado_vtex;
            newSiesaOrder.id_metodo_pago_vtex = unfinishedSiesaOrder.id_metodo_pago_vtex;
            newSiesaOrder.metodo_pago_vtex = unfinishedSiesaOrder.metodo_pago_vtex;
            this.siesaOrdersLocalRepository.updateSiesaOrder(newSiesaOrder);
            return Task.CompletedTask;
        }

        private Task cancelOrder(string orderVtexId)
        {
            this.vtexRepository.cancelOrder(orderVtexId);
            return Task.CompletedTask;
        }

        private Task approvePayment(string paymentId, string orderVtexId)
        {
            this.vtexRepository.approvePayment(paymentId, orderVtexId);
            return Task.CompletedTask;
        }
    }
}
