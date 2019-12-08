using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetModular.Lib.Data.Abstractions;
using NetModular.Lib.Data.Core;
using NetModular.Lib.Data.Query;
using NetModular.Module.Admin.Domain.Account;
using NetModular.Module.Quartz.Domain.Job;
using NetModular.Module.Quartz.Domain.Job.Models;

namespace NetModular.Module.Quartz.Infrastructure.Repositories.SqlServer
{
    public class JobRepository : RepositoryAbstract<JobEntity>, IJobRepository
    {
        public JobRepository(IDbContext context) : base(context)
        {
        }

        public async Task<IList<JobEntity>> Query(JobQueryModel model)
        {
            var paging = model.Paging();

            var query = Db.Find();
            query.WhereNotNull(model.Name, m => m.Name.Contains(model.Name));

            var joinQuery = query.LeftJoin<AccountEntity>((x, y) => x.CreatedBy == y.Id);
            if (!paging.OrderBy.Any())
            {
                joinQuery.OrderByDescending((x, y) => x.Id);
            }

            joinQuery.Select((x, y) => new { x, Creator = y.Name });

            var result = await joinQuery.PaginationAsync(paging);
            model.TotalCount = paging.TotalCount;

            return result;
        }

        public Task<bool> Exists(JobEntity entity)
        {
            var query = Db.Find(m => m.Code == entity.Code && m.Group == entity.Group);
            query.WhereNotEmpty(entity.Id, m => m.Id != entity.Id);

            return query.ExistsAsync();
        }

        public Task<bool> ExistsByGroup(string group)
        {
            return Db.Find(m => m.Group == group).ExistsAsync();
        }

        public Task<bool> UpdateStatus(string group, string code, JobStatus status)
        {
            return Db.Find(m => m.Group == group && m.Code == code).UpdateAsync(m => new JobEntity { Status = status }, false);
        }

        public Task<bool> UpdateStatus(Guid id, JobStatus status, IUnitOfWork uow = null)
        {
            return Db.Find(m => m.Id == id).UseUow(uow).UpdateAsync(m => new JobEntity { Status = status }, false);
        }

        public Task<bool> HasStop(string @group, string code)
        {
            return Db.Find(m => m.Group == group && m.Code == code && m.Status == JobStatus.Stop).ExistsAsync();
        }
    }
}
