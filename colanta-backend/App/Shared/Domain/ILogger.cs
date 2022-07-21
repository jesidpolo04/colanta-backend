namespace colanta_backend.App.Shared.Domain
{
    using System;
    using System.Threading.Tasks;
    public interface ILogger
    {
        Task writelog(Exception exception);
    }
}
