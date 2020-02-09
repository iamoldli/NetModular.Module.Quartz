using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
