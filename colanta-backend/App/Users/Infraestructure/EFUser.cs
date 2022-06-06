namespace colanta_backend.App.Users.Infraestructure
{
    using System;
    using Users.Domain;
    public class EFUser
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public string? last_name { get; set; }
        public string? second_last_name { get; set; }
        public string? address { get; set; }
        public string? country_code { get; set; }
        public string? department_code { get; set; }
        public string? city_code { get; set; }
        public string? document { get; set; }
        public string? document_type { get; set; }
        public string? email { get; set; }
        public string? telephone { get; set; }
        public string? phone { get; set; }
        public string? born_date { get; set; }
        public string? client_type { get; set; }
        public string? business { get; set; }

        public User getUserFromEFUser()
        {
            User user = new User();

            user.id = this.id;
            user.name = this.name;
            user.last_name = this.last_name;
            user.second_last_name = this.second_last_name;
            user.address = this.address;
            user.country_code = this.country_code;
            user.department_code = this.department_code;
            user.city_code = this.city_code;
            user.document = this.document;
            user.document_type = this.document_type;
            user.email = this.email;
            user.telephone = this.telephone;
            user.phone = this.phone;
            user.born_date = this.born_date;
            user.client_type = this.client_type;
            user.business = this.business;

            return user;
        }

        public void setEfUserFromUser(User user)
        {
            this.id = user.id;
            this.name = user.name;
            this.last_name = user.last_name;
            this.second_last_name = user.second_last_name;
            this.address = user.address;
            this.country_code = user.country_code;
            this.department_code = user.department_code;
            this.city_code = user.city_code;
            this.document = user.document;
            this.document_type = user.document_type;
            this.email = user.email;
            this.telephone = user.telephone;
            this.phone = user.phone;
            this.born_date = user.born_date;
            this.client_type = user.client_type;
            this.business = user.business;
        }
    }
}
