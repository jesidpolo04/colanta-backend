namespace colanta_backend.App.Users.Domain
{
    public class User
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public string? document { get; set; }
        public string? document_type { get; set; }
        public string? email { get; set; }
        public string? telephone { get; set; }
        public string? client_type { get; set; }

        public User()
        {
            this.document_type = "CC";
        }
    }
}
