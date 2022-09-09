namespace colanta_backend.App.Orders.Domain
{
    using System;
    public class FailOrderMailLog
    {
        public int id { get; set; }
        public string vtexOrderId { get; set; }
        public DateTime lastMailSend { get; set; }

        public bool alreadyMailSendedAtLast(int units, string timeMeasurementUnit)
        {
            var dif = DateTime.Now.Subtract(lastMailSend);
            if (timeMeasurementUnit.ToUpper() == "H")
            {
                if (dif.TotalHours <= units) return true;
            }
            if(timeMeasurementUnit.ToUpper() == "M")
            {
                if(dif.TotalMinutes <= units) return true;
            }
            return false;
        }

        public void mailHasBeenSend()
        {
            this.lastMailSend = DateTime.Now;
        }
    }
}
