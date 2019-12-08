using System.ComponentModel;

namespace NetModular.Module.Quartz.Domain.JobHttp
{
    /// <summary>
    /// 认证方式
    /// </summary>
    public enum AuthType
    {
        [Description("None")]
        None,
        [Description("Jwt")]
        Jwt
    }
}
