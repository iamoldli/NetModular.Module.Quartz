using System;
using System.Threading.Tasks;
using NetModular.Lib.Data.Abstractions;
using NetModular.Lib.Data.Core;
using NetModular.Module.Quartz.Domain.JobHttp;

namespace NetModular.Module.Quartz.Infrastructure.Repositories.SqlServer
{
    public class JobHttpRepository : RepositoryAbstract<JobHttpEntity>, IJobHttpRepository
    {
        public JobHttpRepository(IDbContext context) : base(context)
        {
        }

        public Task<JobHttpEntity> GetByJob(Guid jobId)
        {
            return GetAsync(m => m.JobId == jobId);
        }
    }
}
