using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetModular.Lib.Module.Abstractions;
using NetModular.Lib.Module.AspNetCore;
using NetModular.Lib.Quartz.Abstractions;
using NetModular.Module.Quartz.Infrastructure;

namespace NetModular.Module.Quartz.Web
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void ConfigureServices(IServiceCollection services, IModuleCollection modules, IHostEnvironment env)
        {
        }

        /// <summary>
        /// 配置中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            var server = app.ApplicationServices.GetService<IQuartzServer>();
            var options = app.ApplicationServices.GetService<QuartzOptions>();
            if (options.Enabled)
            {
                server.Start(options.GetQuartzProps());
            }
        }

        /// <summary>
        /// 配置MVC功能
        /// </summary>
        /// <param name="mvcOptions"></param>
        public void ConfigureMvc(MvcOptions mvcOptions)
        {

        }
    }
}
