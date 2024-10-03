using System.Collections.Generic;
using System.Threading.Tasks;

namespace colanta_backend.App.OrderObservations.Domain
{
    public interface OrderObservationsRepository
    {
        public Task<List<ProductObservationField>> GetOrderObservationFields();

        public Task<List<ProductCutTypeValue>> GetProductCutTypeValues();
    }
}