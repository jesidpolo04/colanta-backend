namespace colanta_backend.App.Products.Infraestructure
{
    using System;
    using Products.Domain;
    using Brands.Domain;
    using Categories.Domain;
    public class SiesaProductsDto
    {
        public SiesaProductDto[] productos { get; set; }
    }

    public class SiesaProductDto
    {
        public int tipo { get; set; }
        public string id { get; set; }
        public string nombre { get; set; }
        public string referencia { get; set; }
        public string? ean { get; set; }
        public string? descripcion { get; set; }
        public string id_marca { get; set; }
        public string id_linea { get; set; }
        public string tipo_producto { get; set; }
        public string unidad_medida { get; set; }
        public decimal iva_aplica { get; set; }
        public string negocio { get; set; }
        public DimensionesDto? dimensiones { get; set; }
        public VariacionDto[] variaciones { get; set; }

        public Product getProductFromDto()
        {
            if (this.tipo == 1)
            {
                Product product = new Product();

                product.type = this.tipo;
                product.siesa_id = this.id;
                product.concat_siesa_id = this.negocio + "_" + this.id;
                product.name = this.nombre;
                product.ref_id = this.referencia;
                product.description = this.descripcion;
                product.business = this.negocio;
                product.setBrand(new Brand(null, id_siesa: this.id_marca));
                product.setCategory(new Category("", siesa_id: this.id_linea));

                Sku sku = new Sku();

                sku.setProduct(product);
                sku.siesa_id = this.id;
                sku.concat_siesa_id = this.negocio + "_" + this.id + "_" + this.id;
                sku.name = this.nombre;
                sku.ref_id = this.referencia;
                sku.description = this.descripcion;
                sku.measurement_unit = this.unidad_medida;

                sku.packaged_height = this.dimensiones.alto_producto;
                sku.packaged_width = this.dimensiones.ancho_producto;
                sku.packaged_length = this.dimensiones.largo_producto;
                sku.packaged_weight_kg = this.dimensiones.peso_bruto;

                product.addSku(sku);

                return product;
            }
            else
            {
                Product product = new Product();

                product.type = this.tipo;
                product.siesa_id = this.id;
                product.concat_siesa_id = this.negocio + "_" + this.id;
                product.name = this.nombre;
                product.ref_id = this.referencia;
                product.description = this.descripcion;
                product.business = this.negocio;
                product.setBrand(new Brand(null, id_siesa: this.id_marca));
                product.setCategory(new Category("", siesa_id: this.id_linea));

                foreach(VariacionDto variacionDto in this.variaciones)
                {
                    Sku sku = new Sku();
                    sku.setProduct(product);
                    sku.siesa_id = variacionDto.id;
                    sku.concat_siesa_id = this.negocio + "_" + this.id + "_" + variacionDto.id;
                    sku.name = variacionDto.nombre;
                    sku.description = this.descripcion;
                    sku.ref_id = variacionDto.referencia + "_" + variacionDto.id;
                    sku.measurement_unit = this.unidad_medida;
                    sku.packaged_height = variacionDto.dimensiones.alto_producto;
                    sku.packaged_width = variacionDto.dimensiones.ancho_producto;
                    sku.packaged_length = variacionDto.dimensiones.largo_producto;
                    sku.packaged_weight_kg = variacionDto.dimensiones.peso_bruto;

                    product.addSku(sku);
                }
                return product;
            }
        }
    }

    public class DimensionesDto
    {
        public double alto_producto { get; set; }
        public double ancho_producto { get; set; }
        public double largo_producto { get; set; }
        public double peso_neto { get; set; }
        public double peso_bruto { get; set; }
    }

    public class VariacionDto
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public string ean { get; set; }
        public string referencia { get; set; }
        public DimensionesDto dimensiones { get; set; }

    }
}
