using NetModular.Lib.Data.Abstractions;

namespace NetModular.Module.Quartz.Infrastructure.Repositories.MySql
{
    public class GroupRepository : SqlServer.GroupRepository
    {
        public GroupRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}