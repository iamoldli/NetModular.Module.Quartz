using NetModular.Lib.Data.Abstractions.Attributes;
using NetModular.Lib.Utils.Core.Extensions;

namespace NetModular.Module.Quartz.Domain.JobHttp
{
    public partial class JobHttpEntity
    {
        [Ignore]
        public string MethodName => Method.ToDescription();

        [Ignore]
        public string AuthTypeName => AuthType.ToDescription();

        [Ignore]
        public string ContentTypeName => ContentType.ToDescription();
    }
}
