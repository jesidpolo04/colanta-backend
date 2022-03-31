namespace colanta_backend.App.Shared.Domain
{
    using System;
    public class Process
    {
        public int id { get; set; }
        public string name { get; set; }
        public int total_loads { get; set; }
        public int total_errors { get; set; }
        public int total_not_procecced { get; set; }
        public string json_details { get; set; }
        public DateTime dateTime { get; set; }
    }
}
