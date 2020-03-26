using System.ComponentModel;

namespace NetModular.Module.Quartz.Infrastructure.Core
{
    public enum QuartzSerializerType
    {
        [Description("JSON")]
        Json,
        [Description("XML")]
        Xml
    }
}
