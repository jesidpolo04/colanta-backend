namespace colanta_backend.App.Specifications.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface SpecificationsVtexRepository
    {
        Task<List<Specification>> getProductSpecifications(int productVtexId);
        Task<Specification> updateProductSpecification(int productVtexId, Specification specification);
    }
}
