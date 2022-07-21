namespace colanta_backend.App.Shared.Application
{
    public interface IProcess
    {
        public void Log(string name, int total_loads, int total_errors, int total_not_procecced, int total_obtained, string? json_details = null);
    }
}
