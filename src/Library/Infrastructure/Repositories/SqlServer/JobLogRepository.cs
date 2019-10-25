using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetModular.Lib.Data.Abstractions;
using NetModular.Lib.Data.Core;
using NetModular.Lib.Data.Query;
using NetModular.Module.Quartz.Domain.JobLog;
using NetModular.Module.Quartz.Domain.JobLog.Models;

namespace NetModular.Module.Quartz.Infrastructure.Repositories.SqlServer
{
    public class JobLogRepository : RepositoryAbstract<JobLogEntity>, IJobLogRepository
    {
        public JobLogRepository(IDbContext context) : base(context)
        {
        }

        public async Task<IList<JobLogEntity>> Query(JobLogQueryModel model)
        {
            var paging = model.Paging();

            var query = Db.Find();
            query.Where(m => m.JobId == model.JobId);
            query.WhereNotNull(model.StartDate, m => m.CreatedTime >= model.StartDate.Value);
            query.WhereNotNull(model.EndDate, m => m.CreatedTime < model.EndDate.Value.AddDays(1));

            if (!paging.OrderBy.Any())
            {
                query.OrderByDescending(m => m.Id);
            }

            var result = await query.PaginationAsync(paging);
            model.TotalCount = paging.TotalCount;

            return result;
        }
    }
}
