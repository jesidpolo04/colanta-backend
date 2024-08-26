namespace colanta_backend.App.Orders.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface CanceledOrdersRepository
    {
        Task<List<CanceledOrder>> GetAll();
    }
}
