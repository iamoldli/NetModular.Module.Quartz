using System.Collections.Generic;
using System.Threading.Tasks;
using NetModular.Lib.Data.Abstractions;
using NetModular.Module.Quartz.Domain.Group.Models;

namespace NetModular.Module.Quartz.Domain.Group
{
    /// <summary>
    /// 任务组仓储
    /// </summary>
    public interface IGroupRepository : IRepository<GroupEntity>
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IList<GroupEntity>> Query(GroupQueryModel model);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> Exists(GroupEntity entity);
    }
}
