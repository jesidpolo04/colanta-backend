namespace colanta_backend.App.GiftCards.Domain
{
    using System;
    using System.Security.Cryptography;
    public class Transaction
    {
        public string id { get; set; }
        public string card_id { get; set; }
        public DateTime date { get; set; }

        public Transaction(string cardId) 
        {
            this.card_id = cardId;
            this.date = DateTime.Now;
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(card_id + date.ToString());
            this.id = Convert.ToBase64String(bytes);
        }
    }
}
