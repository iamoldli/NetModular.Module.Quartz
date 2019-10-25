using NetModular.Lib.Data.Abstractions;

namespace NetModular.Module.Quartz.Infrastructure.Repositories.SQLite
{
    public class JobRepository : SqlServer.JobRepository
    {
        public JobRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
