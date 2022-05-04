namespace colanta_backend.App.Users.Infraestructure
{
    using Users.Domain;
    public class EFUser
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public string? document { get; set; }
        public string? document_type { get; set; }
        public string? email { get; set; }
        public string? telephone { get; set; }
        public string? client_type { get; set; }

        public User getUserFromEFUser()
        {
            User user = new User();

            user.id = this.id;
            user.name = this.name;
            user.document = this.document;
            user.document_type = this.document_type;
            user.email = this.email;
            user.telephone = this.telephone;
            user.client_type = this.client_type;

            return user;
        }

        public void setEfUserFromUser(User user)
        {
            this.id = user.id;
            this.name = user.name;
            this.document = user.document;
            this.document_type = user.document_type;
            this.email = user.email;
            this.telephone = user.telephone;
            this.client_type = user.client_type;
        }
    }
}
