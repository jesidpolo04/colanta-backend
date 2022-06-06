namespace colanta_backend.App.Users.Domain
{
    using System;
    public class User
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

        public User()
        {
            this.document_type = "CC";
        }
    }
}
