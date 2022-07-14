using colanta_backend.App.Orders.Domain;


namespace colanta_backend.App.Orders.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using Shared.Infraestructure;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using System.Linq;
    public class PaymentMethodsEFRepository : Domain.PaymentMethodsRepository
    {
        private ColantaContext dbContext;
        public PaymentMethodsEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }
        public async Task<PaymentMethod> getPaymentMethodByName(string name)
        {
            EFPaymentMethod[] efPaymentMethods = this.dbContext.PaymentMethods.Where(pm => pm.name == name).ToArray();
            if(efPaymentMethods.Length > 0)
            {
                return efPaymentMethods.First().getPaymentFromEfPayment();
            }
            return null;
        }

        public async Task<PaymentMethod> getPaymentMethodByVtexId(string vtex_id)
        {
            EFPaymentMethod[] efPaymentMethods = this.dbContext.PaymentMethods.Where(pm => pm.vtex_id == vtex_id).ToArray();
            if (efPaymentMethods.Length > 0)
            {
                return efPaymentMethods.First().getPaymentFromEfPayment();
            }
            return null;
        }

        public async Task<PaymentMethod[]> getPaymentMethosByIsPromissory(bool isPromissory)
        {
            EFPaymentMethod[] efPaymentMethods = this.dbContext.PaymentMethods.Where(pm => pm.is_promissory == isPromissory).ToArray();
            List<PaymentMethod> paymentMethods = new List<PaymentMethod>();
            foreach(EFPaymentMethod efPaymentMethod in efPaymentMethods)
            {
                paymentMethods.Add(efPaymentMethod.getPaymentFromEfPayment());
            }
            return paymentMethods.ToArray();
        }
    }
}
