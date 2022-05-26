namespace colanta_backend.App.Shared.Infraestructure
{
    using System;
    using Shared.Domain;
    public partial class EFProcess
    {
        public int id { get; set; }
        public string name { get; set;}
        public int total_loads { get; set; }
        public int total_errors  { get; set; }
        public int total_not_procecced { get; set; }
        public int total_obtained { get; set; }
        public string json_details  { get; set; }
        public DateTime dateTime { get; set; }

        public Process getProcessFromEfProcess()
        {
            Process process = new Process();
            process.id = this.id;
            process.name = this.name;
            process.total_loads = this.total_loads;
            process.total_errors = this.total_errors;
            process.total_not_procecced = this.total_not_procecced;
            process.total_obtained = this.total_obtained;
            process.json_details = this.json_details;
            process.dateTime = this.dateTime;
            return process;
        }

        public void setProcessFromProcess(Process process)
        {
            this.id = process.id;
            this.name = process.name;
            this.total_loads = process.total_loads;
            this.total_errors = process.total_errors;
            this.total_not_procecced = process.total_not_procecced;
            this.total_obtained = process.total_obtained;
            this.dateTime = process.dateTime;
            this.json_details= process.json_details;
        }
    }
}
