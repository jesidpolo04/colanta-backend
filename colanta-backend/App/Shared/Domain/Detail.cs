namespace colanta_backend.App.Shared.Domain
{
    public class Detail
    {
        
        public string api_endpoint { get; set; }
        public string origin { get; set; }
        public string content { get; set; }
        public string description { get; set; }
        public bool success { get; set; }

        public Detail(string api_endpoint = null, string origin= null, string content = null, string description = null, bool success = true)
        {
            this.api_endpoint = api_endpoint;
            this.origin = origin;
            this.content = content;
            this.description = description;
            this.success = success;
        }

    }
}
