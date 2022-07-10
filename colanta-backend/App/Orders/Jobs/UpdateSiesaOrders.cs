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
        private SiesaOrdersRepository siesaOrdersLocalRepository;
        private SiesaOrdersHistoryRepository siesaOrdersHistoryLocalRepository;
        private OrdersSiesaRepository siesaRepository;
        private OrdersVtexRepository vtexRepository;

        public UpdateSiesaOrders(
            OrdersRepository localRepository,
            SiesaOrdersRepository siesaOrdersLocalRepository,
            SiesaOrdersHistoryRepository siesaOrdersHistoryLocalRepository,
            OrdersSiesaRepository siesaRepository,
            OrdersVtexRepository vtexRepository
            )
        {
            this.localRepository = localRepository;
            this.siesaOrdersLocalRepository = siesaOrdersLocalRepository;
            this.siesaOrdersHistoryLocalRepository = siesaOrdersHistoryLocalRepository;
            this.siesaRepository = siesaRepository;
            this.vtexRepository = vtexRepository;
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
                        if (newSiesaOrder.finalizado)
                        {
                            await this.siesaOrdersHistoryLocalRepository.saveSiesaOrderHistory(unfinishedSiesaOrder);
                            newSiesaOrder.siesa_id = unfinishedSiesaOrder.siesa_id;
                            await this.siesaOrdersLocalRepository.updateSiesaOrder(newSiesaOrder);
                        }
                    }
                    catch(SiesaException siesaException)
                    {

                    }
                }

            }
            catch (Exception exception)
            {

            }
        }
    }
}
