namespace colanta_backend.App.Shared.Domain
{
    public class Detail
    {
        public string origin { get; set; }
        public string action { get; set; }
        public string content { get; set; }
        public string description { get; set; }
        public bool success { get; set; }

        public Detail(string origin = null, string action = null, string content = null, string description = null, bool success = true)
        {
            this.origin = origin;
            this.action = action;
            this.content = content;
            this.description = description;
            this.success = success;
        }

    }
}
