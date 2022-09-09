namespace colanta_backend.App.Orders.Infraestructure
{
    using Shared.Infraestructure;
    using Orders.Domain;
    using System.Threading.Tasks;
    using System.Linq;
    using Microsoft.Extensions.Configuration;

    public class FailOrderMailLogsEFRepository : FailOrderMailLogsRepository
    {
        private ColantaContext dbContext;
        public FailOrderMailLogsEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }
        public async Task<FailOrderMailLog> getLogByOrderVtexId(string orderVtexId)
        {
            var efLogs = this.dbContext.FailOrderMailLogs.Where(log => log.vtexOrderId == orderVtexId);
            if(efLogs.ToArray().Length > 0)
            {
                return efLogs.First().getFailOrderMailLog();
            }
            return null;
        }

        public Task saveLog(FailOrderMailLog newLog)
        {
            var efLog = new EFFailOrderMailLog();
            efLog.setEFFailOrderMailLog(newLog);
            this.dbContext.Add(efLog);
            this.dbContext.SaveChanges();
            return Task.CompletedTask;
        }

        public Task updateLog(FailOrderMailLog log)
        {
            var efLog = this.dbContext.FailOrderMailLogs.Find(log.id);
            efLog.lastMailSend = log.lastMailSend;
            efLog.vtexOrderId = log.vtexOrderId;
            this.dbContext.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
