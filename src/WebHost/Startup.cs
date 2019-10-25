using Microsoft.Extensions.Hosting;
using NetModular.Lib.Host.Web;

namespace NetModular.Module.Common.WebHost
{
    public class Startup : StartupAbstract
    {
        public Startup(IHostEnvironment env) : base(env)
        {
        }
    }
}
