using NetModular.Lib.Data.Abstractions.Attributes;
using NetModular.Lib.Utils.Core.Extensions;

namespace NetModular.Module.Quartz.Domain.Job
{
    public partial class JobEntity
    {
        /// <summary>
        /// 任务类型名称
        /// </summary>
        [Ignore]
        public string TypeName => JobType.ToDescription();
    }
}
