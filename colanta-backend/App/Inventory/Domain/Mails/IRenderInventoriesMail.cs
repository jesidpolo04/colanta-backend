namespace colanta_backend.App.Inventory.Domain
{
    public interface IRenderInventoriesMail
    {
        void sendMail(string subject, string templatePath, object model);

    }
}
