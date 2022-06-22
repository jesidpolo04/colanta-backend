namespace colanta_backend.App.Orders.Infraestructure
{
    public class VtexOrderDto
    {
        public ShippingDataDto shippingData { get; set; }
        public string orderId { get; set; }
        public string creationDate { get; set; }
        public ClientProfileDataDto clientProfileData { get; set; }
        public PaymentDataDto paymentData { get; set; }
        public string salesChannel { get; set; }
        public TotalDto[] totals { get; set; }
        public ItemDto[] items { get; set; }
        
    }
    public class ItemDto
    {
        public string refId { get; set; }
        public bool isGift { get; set; }
        public string measurementUnit { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public string productId { get; set; }
        public string id { get; set; }
        public PriceTagDto[] priceTags { get; set; }

    }
    public class PriceTagDto
    {
        public string identifier { get; set; }
        public decimal value { get; set; }
    }

        public class TotalDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public decimal value { get; set; }
    }
    public class ShippingDataDto
    {
        public LogisticInfoDto[] logisticsInfo { get; set; }
        public SelectedAddresseDto[] selectedAddresses { get; set; }
        public string shippingEstimateDate { get; set; }
    }
    public class SelectedAddresseDto 
    {
        public string street { get; set; }
        public string complement { get; set; }
        public string neighborhood { get; set; }
        public string reference { get; set; }
    }
    public class LogisticInfoDto
    {
        public DeliveryIdDto[] deliveryIds { get; set; }
    }
    public class DeliveryIdDto
    {
        public string warehouseId { get; set; }
    }
    public class ClientProfileDataDto
    {
        public string document { get; set; }
    }
    public class PaymentDataDto
    {
        public TransactionDto[] transactions { get; set; }
    }
    public class TransactionDto
    {
        public PaymentDto[] payments { get; set; }
    }
    public class PaymentDto
    {
        public string paymentSystem { get; set; }
    }
}
