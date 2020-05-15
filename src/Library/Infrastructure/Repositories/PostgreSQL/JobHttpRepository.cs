using NetModular.Lib.Data.Abstractions;

namespace NetModular.Module.Quartz.Infrastructure.Repositories.PostgreSQL
{
    public class JobHttpRepository : SqlServer.JobHttpRepository
    {
        public JobHttpRepository(IDbContext context) : base(context)
        {
        }
    }
}
