namespace colanta_backend.App.Shared.Application
{
    public interface ILogs
    {
        public void Log(string name, int total_loads, int total_errors, int total_not_procecced, string? json_details = null);
    }
}
