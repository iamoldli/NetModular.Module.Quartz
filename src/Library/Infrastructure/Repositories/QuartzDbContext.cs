using NetModular.Lib.Data.Abstractions;
using NetModular.Lib.Data.Core;

namespace NetModular.Module.Quartz.Infrastructure.Repositories
{
    public class QuartzDbContext : DbContext
    {
        public QuartzDbContext(IDbContextOptions options) : base(options)
        {
        }
    }
}
