using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetModular.Lib.Module.Abstractions;
using NetModular.Lib.Quartz.Abstractions;
using NetModular.Lib.Quartz.Core;
using NetModular.Module.Quartz.Infrastructure.Core;
using Quartz;

namespace NetModular.Module.Quartz.Infrastructure
{
    public class ModuleServicesConfigurator : IModuleServicesConfigurator
    {
        public void Configure(IServiceCollection services, IModuleCollection modules, IHostEnvironment env, IConfiguration cfg)
        {
            services.AddSingleton<ITaskLogger, Core.TaskLogger>();
            services.AddSingleton<ISchedulerListener, SchedulerListener>();
            services.AddQuartz(modules);
        }
    }
}
