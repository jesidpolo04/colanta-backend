namespace colanta_backend.App.Orders.Domain
{
    using System;
    using System.Threading.Tasks;
    using Shared.Domain;
    using Orders.Domain;
    using Orders.SiesaOrders.Domain;
    public class MailService
    {
        private INewOrderMail newOrderMail;
        private ISiesaErrorAtSendOrderMail siesaErrorAtSendOrderMail;
        private FailOrderMailLogsRepository failOrderMailLogsRepository;

        public MailService(
            INewOrderMail newOrderMail,
            ISiesaErrorAtSendOrderMail siesaErrorMail,
            FailOrderMailLogsRepository failOrderMailLogsRepository
            )
        {
            this.newOrderMail = newOrderMail;
            this.siesaErrorAtSendOrderMail = siesaErrorMail;
            this.failOrderMailLogsRepository = failOrderMailLogsRepository;
        }

        public void SendMailToWarehouse(string wharehouseId, SiesaOrder siesaOrder, VtexOrder vtexOrder)
        {
            this.newOrderMail.SendMailToWarehouse(wharehouseId, siesaOrder, vtexOrder);
        }

        public void SendSiesaErrorMail(SiesaException exception, string vtexOrderId)
        {
            this.siesaErrorAtSendOrderMail.SendMail(exception, vtexOrderId);
        }

        public bool alreadyFailOrderMailSendedAtLast(int unit, string timeMeasurementUnit, string vtexOrderId)
        {
            var failOrderMailLog = this.failOrderMailLogsRepository.getLogByOrderVtexId(vtexOrderId).Result;
            if (failOrderMailLog == null)
            {
                return false;
            }
            return failOrderMailLog.alreadyMailSendedAtLast(unit, timeMeasurementUnit);
        }

        public Task createOrUpdateFailOrderMailLog(string vtexOrderId)
        {
            var log = this.failOrderMailLogsRepository.getLogByOrderVtexId(vtexOrderId).Result;
            if (log == null)
            {
                this.saveFailOrderMailLog(vtexOrderId);
                return Task.CompletedTask;
            }
            else
            {
                this.updateFailOrderMailLog(vtexOrderId);
                return Task.CompletedTask;
            }
                
        }
        private void saveFailOrderMailLog(string vtexOrderId)
        {
            var failOrderMailLog = new FailOrderMailLog();
            failOrderMailLog.vtexOrderId = vtexOrderId;
            failOrderMailLog.lastMailSend = DateTime.Now;
            this.failOrderMailLogsRepository.saveLog(failOrderMailLog);
        }

        private void updateFailOrderMailLog(string vtexOrderId)
        {
            var log = this.failOrderMailLogsRepository.getLogByOrderVtexId(vtexOrderId).Result;
            log.lastMailSend = DateTime.Now;
            this.failOrderMailLogsRepository.updateLog(log);
        }

    }
}
