namespace colanta_backend.App.Orders.Infraestructure
{
    using Orders.Domain;
    using Inventory.Domain;
    using Orders.SiesaOrders.Domain;
    using Shared.Domain;
    public class NewOrderMail : INewOrderMail
    {
        private EmailSender emailSender;
        private WarehousesRepository warehouseRepository;
        private string subject;
        private string template;
        public NewOrderMail(EmailSender emailSender, WarehousesRepository warehousesRepository)
        {
            this.emailSender = emailSender;
            this.warehouseRepository = warehousesRepository;
            this.template = "./App/Orders/Infraestructure/Mails/NewOrderMail.cshtml";
        }

        public void SendMailToWarehouse(string wharehouseId, SiesaOrder siesaOrder)
        {
            this.subject = $"Tienes un nuevo pedido: #{siesaOrder.referencia_vtex}";
            NewOrderMailModel model = new NewOrderMailModel(siesaOrder);
            Warehouse warehouse = this.warehouseRepository.getWarehouseBySiesaId(wharehouseId).Result;
            this.emailSender.SendEmail(this.subject, this.template, model, warehouse.email);
            //this.emailSender.SendEmail(this.subject, this.template, model, "cristianro@colanta.com.co");
            //this.emailSender.SendEmail(this.subject, this.template, model, "pvtcar1@colanta.com.co");
            //this.emailSender.SendEmail(this.subject, this.template, model, "pidecolanta@colanta.com.co");
        }
    }
}
