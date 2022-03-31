namespace colanta_backend.App.Brands.Infraestructure
{
    using Brands.Domain;
    public class VtexBrandMapper
    {
        public Brand dtoToBrand(VtexBrandDTO vtexBrandDto)
        {
            return new Brand(id_vtex: vtexBrandDto.Id, name: vtexBrandDto.Name, state: vtexBrandDto.Active);
        }
    }
}
