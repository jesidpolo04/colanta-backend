namespace colanta_backend.App.OrderObservations.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using colanta_backend.App.OrderObservations.Domain;
    using colanta_backend.App.Shared.Infraestructure;
    using Microsoft.EntityFrameworkCore;
    
    public class OrderObservationsEFRepository : OrderObservationsRepository
    {
        private readonly ColantaContext _context;

        public OrderObservationsEFRepository(ColantaContext context)
        {
            _context = context;
        }

        public async Task<List<ProductObservationField>> GetOrderObservationFields()
        {
            return await _context.ProductObservationFields.Select(pof => pof.GetProductObservationField()).ToListAsync();
        }

        public async Task<List<ProductCutTypeValue>> GetProductCutTypeValues()
        {
            return await _context.ProductCutTypeValues.Select(pct => pct.GetProductCutType()).ToListAsync();
        }
    }
}