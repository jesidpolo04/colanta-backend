namespace colanta_backend.App.Orders.Infraestructure
{
    using System;
    using Orders.Domain;
    public class EFFailOrderMailLog
    {
        public int id { get; set; }
        public string vtexOrderId { get; set; }
        public DateTime lastMailSend { get; set; }

        public void setEFFailOrderMailLog(FailOrderMailLog log)
        {
            this.id = log.id;
            this.vtexOrderId = log.vtexOrderId;
            this.lastMailSend = log.lastMailSend;
        }

        public FailOrderMailLog getFailOrderMailLog()
        {
            var log = new FailOrderMailLog();
            log.id = this.id;
            log.vtexOrderId = this.vtexOrderId;
            log.lastMailSend = this.lastMailSend;
            return log;
        }
    }
}
