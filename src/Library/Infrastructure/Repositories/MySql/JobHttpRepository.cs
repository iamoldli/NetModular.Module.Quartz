using NetModular.Lib.Data.Abstractions;

namespace NetModular.Module.Quartz.Infrastructure.Repositories.MySql
{
    public class JobHttpRepository : SqlServer.JobHttpRepository
    {
        public JobHttpRepository(IDbContext context) : base(context)
        {
        }
    }
}
