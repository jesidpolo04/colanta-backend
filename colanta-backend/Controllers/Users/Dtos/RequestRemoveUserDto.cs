namespace colanta_backend.Controllers.Users
{
    public class RequestRemoveUserDto
    {
        public string email { get; set; } 
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string document { get; set; }
        public string? documentType { get; set; }
        public string? phone {get; set; }
    }
}
