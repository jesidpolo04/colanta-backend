namespace colanta_backend.App.Orders.Domain
{
    using System.Threading.Tasks;
    public interface PaymentMethodsRepository
    {
        Task<PaymentMethod> getPaymentMethodByVtexId(string vtex_id);
        Task<PaymentMethod> getPaymentMethodByName(string name);
        Task<PaymentMethod[]> getPaymentMethosByIsPromissory(bool isPromissory);
    }
}
