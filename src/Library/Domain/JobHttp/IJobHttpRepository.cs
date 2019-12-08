using System;
using System.Threading.Tasks;
using NetModular.Lib.Data.Abstractions;

namespace NetModular.Module.Quartz.Domain.JobHttp
{
    public interface IJobHttpRepository : IRepository<JobHttpEntity>
    {
        /// <summary>
        /// 根据任务编号查询
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task<JobHttpEntity> GetByJob(Guid jobId);
    }
}
