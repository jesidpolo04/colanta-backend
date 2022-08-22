namespace colanta_backend.App.Users.Domain
{
    public class VtexUser
    {
        public string id {get; set;}
        public string document {get; set;}
        public string? documentType { get; set;}
        public string email {get; set;}
        public string firstName {get; set;}
        public string lastName {get; set;}
        public string? phone {get; set;}
        public string? homePhone {get; set;}
        public string? birthDate { get; set; }
    }
}
