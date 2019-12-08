using System.ComponentModel;

namespace NetModular.Module.Quartz.Domain.Job
{
    /// <summary>
    /// 任务类型
    /// </summary>
    public enum JobType
    {
        /// <summary>
        /// 通用
        /// </summary>
        [Description("通用")]
        Normal,
        /// <summary>
        /// Http
        /// </summary>
        [Description("HTTP")]
        Http
    }
}
