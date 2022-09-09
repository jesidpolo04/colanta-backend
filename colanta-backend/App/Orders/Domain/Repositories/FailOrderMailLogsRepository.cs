namespace colanta_backend.App.Orders.Domain
{
    using System.Threading.Tasks;
    public interface FailOrderMailLogsRepository
    {
        Task<FailOrderMailLog> getLogByOrderVtexId(string orderVtexId);
        Task updateLog(FailOrderMailLog log);
        Task saveLog(FailOrderMailLog newLog);
    }
}
