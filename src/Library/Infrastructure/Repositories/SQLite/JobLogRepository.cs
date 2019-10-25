using NetModular.Lib.Data.Abstractions;

namespace NetModular.Module.Quartz.Infrastructure.Repositories.SQLite
{
    public class JobLogRepository : SqlServer.JobLogRepository
    {
        public JobLogRepository(IDbContext context) : base(context)
        {
        }
    }
}
