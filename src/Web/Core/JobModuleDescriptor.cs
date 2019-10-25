using System.Collections.Generic;
using NetModular.Lib.Module.Abstractions;
using NetModular.Lib.Utils.Core.Result;

namespace NetModular.Module.Quartz.Web.Core
{
    /// <summary>
    /// 任务模块描述类
    /// </summary>
    public class JobModuleDescriptor
    {
        /// <summary>
        /// 模块ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IModuleDescriptor Descriptor { get; set; }

        /// <summary>
        /// 任务类下拉列表
        /// </summary>
        public IList<OptionResultModel> JobClassSelect { get; set; }
    }
}
