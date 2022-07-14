namespace colanta_backend.App.Orders.Jobs
{
    using Orders.Domain;
    using Shared.Domain;
    using SiesaOrders.Domain;
    using System;
    using System.Threading.Tasks;
    public class UpdateSiesaOrders
    {
        private OrdersRepository localRepository;
        private PaymentMethodsRepository paymentMethodsLocalRepository;
        private SiesaOrdersRepository siesaOrdersLocalRepository;
        private SiesaOrdersHistoryRepository siesaOrdersHistoryLocalRepository;
        private OrdersSiesaRepository siesaRepository;
        private OrdersVtexRepository vtexRepository;
        private GetOrderDetailsVtexId getOrderDetailsVtexId;

        public UpdateSiesaOrders(
            OrdersRepository localRepository,
            PaymentMethodsRepository paymentMethodsLocalRepository,
            SiesaOrdersRepository siesaOrdersLocalRepository,
            SiesaOrdersHistoryRepository siesaOrdersHistoryLocalRepository,
            OrdersSiesaRepository siesaRepository,
            OrdersVtexRepository vtexRepository,
            GetOrderDetailsVtexId getOrderDetailsVtexId
            )
        {
            this.localRepository = localRepository;
            this.paymentMethodsLocalRepository = paymentMethodsLocalRepository;
            this.siesaOrdersLocalRepository = siesaOrdersLocalRepository;
            this.siesaOrdersHistoryLocalRepository = siesaOrdersHistoryLocalRepository;
            this.siesaRepository = siesaRepository;
            this.vtexRepository = vtexRepository;
            this.getOrderDetailsVtexId = getOrderDetailsVtexId;
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
                        await getOrderDetailsVtexId.Invoke(newSiesaOrder);
                        if (newSiesaOrder.finalizado)
                        {
                            await this.siesaOrdersHistoryLocalRepository.saveSiesaOrderHistory(unfinishedSiesaOrder);

                            newSiesaOrder.siesa_id = unfinishedSiesaOrder.siesa_id;
                            newSiesaOrder.estado_vtex = unfinishedSiesaOrder.estado_vtex;
                            newSiesaOrder.id_metodo_pago_vtex = unfinishedSiesaOrder.id_metodo_pago_vtex;
                            newSiesaOrder.metodo_pago_vtex = unfinishedSiesaOrder.metodo_pago_vtex;
                            PaymentMethod paymentMethod = await this.paymentMethodsLocalRepository.getPaymentMethodByVtexId(newSiesaOrder.id_metodo_pago_vtex);
                            bool isPromissoryPaymentMethod = paymentMethod != null ? paymentMethod.is_promissory : false;
                            if(newSiesaOrder.estado_vtex == "ready-for-handling")
                            {
                                await vtexRepository.startHandlingOrder(newSiesaOrder.referencia_vtex);
                                newSiesaOrder.estado_vtex = "handling";
                            }
                            if (isPromissoryPaymentMethod && newSiesaOrder.estado_vtex == "handling") //metodo de pago promisorio
                            {
                                await this.vtexRepository.updateVtexOrder(unfinishedSiesaOrder, newSiesaOrder);
                            }
                            await this.siesaOrdersLocalRepository.updateSiesaOrder(newSiesaOrder);
                        }
                    }
                    catch(SiesaException siesaException)
                    {
                        Console.WriteLine(siesaException.Message);
                        
                    }
                    catch(VtexException vtexException)
                    {
                        Console.WriteLine(vtexException.Message);
                    }
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
