using System.ComponentModel.DataAnnotations;

namespace NetModular.Module.Quartz.Application.JobService.ViewModels
{
    /// <summary>
    /// 任务添加模型
    /// </summary>
    public class JobAddModel : JobBaseModel
    {
        /// <summary>
        /// 任务类
        /// </summary>
        [Required(ErrorMessage = "请选择任务类")]
        public string JobClass { get; set; }

        /// <summary>
        /// 立即启动
        /// </summary>
        public bool Start { get; set; }
    }
}
