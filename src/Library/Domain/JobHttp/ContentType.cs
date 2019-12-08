using System.ComponentModel;

namespace NetModular.Module.Quartz.Domain.JobHttp
{
    /// <summary>
    /// 数据格式
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// application/json
        /// </summary>
        [Description("application/json")]
        Json,
        /// <summary>
        /// application/x-www-form-urlencoded
        /// </summary>
        [Description("application/x-www-form-urlencoded")]
        Form,
        /// <summary>
        /// text/plain
        /// </summary>
        [Description("text/plain")]
        Text,
        /// <summary>
        /// text/html
        /// </summary>
        [Description("text/html")]
        Html,
        /// <summary>
        /// application/xml
        /// </summary>
        [Description("application/xml")]
        Xml
    }
}
