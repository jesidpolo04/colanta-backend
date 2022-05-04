namespace colanta_backend.Controllers.Users
{
    using App.Users.Domain;
    public class RequestUserDto
    {
        public string document { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string? telephone { get; set; }

        public User getUserDto()
        {
            User user = new User();

            user.document = document;
            user.name = name;
            user.telephone = telephone;
            user.email = email;

            return user;
        }
    }
}
