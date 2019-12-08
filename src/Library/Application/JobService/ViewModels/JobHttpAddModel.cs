using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NetModular.Lib.Utils.Core.Enums;
using NetModular.Module.Quartz.Domain.JobHttp;

namespace NetModular.Module.Quartz.Application.JobService.ViewModels
{
    /// <summary>
    /// Http任务添加模型
    /// </summary>
    public class JobHttpAddModel : JobBaseModel
    {
        /// <summary>
        /// 请求Url
        /// </summary>
        [Required(ErrorMessage = "请输入请求地址")]
        [Url(ErrorMessage = "请输入正确URL")]
        public string Url { get; set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// 认证方式
        /// </summary>
        public AuthType AuthType { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 数据格式
        /// </summary>
        public ContentType ContentType { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// 请求头字典
        /// </summary>
        public List<KeyValuePair<string, string>> HeaderList { get; set; } = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// 立即启动
        /// </summary>
        public bool Start { get; set; }
    }
}
