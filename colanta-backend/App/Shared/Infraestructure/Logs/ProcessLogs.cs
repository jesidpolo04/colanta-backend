namespace colanta_backend.App.Shared.Infraestructure
{
    using App.Shared.Application;
    public class ProcessLogs : ILogs
    {
        private colantaContext dbContext;
        public ProcessLogs()
        {
            this.dbContext = new colantaContext();
        }
        public void Log(string name, int total_loads, int total_errors, int total_not_procecced, string? json_details = null)
        {
            EFProcess efProcess = new EFProcess();
            efProcess.name = name;
            efProcess.total_loads = total_loads;
            efProcess.total_errors = total_errors;
            efProcess.total_not_procecced = total_not_procecced;
            efProcess.json_details = json_details;
            this.dbContext.Process.Add(efProcess);
            this.dbContext.SaveChanges();
        }
    }
}
