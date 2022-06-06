namespace colanta_backend.Controllers.Users
{
    using System;
    using App.Users.Domain;
    public class RequestUserDto
    {
        public string document { get; set; }
        public string name { get; set; }
        public string last_name { get; set; }
        public string? second_last_name { get; set; }
        public string? phone { get; set; }
        public string country_code { get; set; }
        public string department_code { get; set; }
        public string city_code { get; set; }
        public string email { get; set; }
        public string born_date { get; set; }
        public string? telephone { get; set; }
        public string? store { get; set; } //business

        public User getUserDto()
        {
            User user = new User();

            user.document = document;
            user.name = name;
            user.last_name = last_name;
            user.second_last_name = second_last_name;
            user.telephone = telephone;
            user.phone = phone;
            user.country_code = country_code;
            user.department_code = department_code;
            user.city_code = city_code;
            user.email = email;
            user.born_date = born_date;
            user.business = store;
            return user;
        }
    }
}
