using System;
using NetModular.Lib.Data.Abstractions.Attributes;
using NetModular.Lib.Data.Core.Entities;
using NetModular.Lib.Utils.Core.Enums;

namespace NetModular.Module.Quartz.Domain.JobHttp
{
    /// <summary>
    /// Http任务信息
    /// </summary>
    [Table("Job_Http")]
    public partial class JobHttpEntity : Entity<int>
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public Guid JobId { get; set; }

        /// <summary>
        /// 请求Url
        /// </summary>
        [Nullable]
        [Length(500)]
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
        [Nullable]
        [Length(300)]
        public string Token { get; set; }

        /// <summary>
        /// 数据格式
        /// </summary>
        public ContentType ContentType { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        [Max]
        [Nullable]
        public string Parameters { get; set; }

        /// <summary>
        /// 请求头(json格式)
        /// </summary>
        [Max]
        [Nullable]
        public string Headers { get; set; }
    }
}
