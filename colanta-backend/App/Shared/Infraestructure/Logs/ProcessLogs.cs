namespace colanta_backend.App.Shared.Infraestructure
{
    using App.Shared.Application;
    using Microsoft.Extensions.Configuration;
    public class ProcessLogs : IProcess
    {
        private ColantaContext dbContext;
        public ProcessLogs(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }
        public void Log(string name, int total_loads, int total_errors, int total_not_procecced, int total_obtained, string? json_details = null)
        {
            EFProcess efProcess = new EFProcess();
            efProcess.name = name;
            efProcess.total_loads = total_loads;
            efProcess.total_errors = total_errors;
            efProcess.total_not_procecced = total_not_procecced;
            efProcess.total_obtained = total_obtained;
            efProcess.json_details = json_details;
            this.dbContext.Process.Add(efProcess);
            this.dbContext.SaveChanges();
        }
    }
}
