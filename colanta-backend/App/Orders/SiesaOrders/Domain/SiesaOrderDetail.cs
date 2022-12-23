namespace colanta_backend.App.Orders.SiesaOrders.Domain
{
    using System;
    public class SiesaOrderDetail
    {
        public int id { get; set; }
        public string det_co { get; set; }
        public int nro_detalle { get; set; }
        public string referencia_item { get; set; }
        public string variacion_item { get; set; }
        public int ind_obsequio { get; set; }
        public string unidad_medida { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public string notas { get; set; }
        public decimal impuesto { get; set; }
        public string referencia_vtex { get; set; }
        public int order_id { get; set; }
        public SiesaOrder order { get; set; }

        public bool thisItemExists(SiesaOrderDetail[] newOrderDetails)
        {
            bool exists = false;
            foreach (SiesaOrderDetail newOrderDetail in newOrderDetails)
            {
                if(this.referencia_item == newOrderDetail.referencia_item)
                {
                    exists = true;
                    break;
                }
            }
            return exists;
        }
    }
}
