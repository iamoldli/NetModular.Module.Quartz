using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetModular.Lib.Config.Abstractions;
using NetModular.Lib.Data.Abstractions.Enums;
using NetModular.Lib.Data.Abstractions.Options;
using NetModular.Lib.Module.Abstractions;
using NetModular.Lib.Module.AspNetCore;
using NetModular.Lib.Quartz.Abstractions;
using Newtonsoft.Json;

namespace NetModular.Module.Quartz.Web
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void ConfigureServices(IServiceCollection services, IModuleCollection modules, IHostEnvironment env, IConfiguration cfg)
        {
        }

        /// <summary>
        /// 配置中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            //使用当前模块的数据库配置信息来配置Quartz的数据库配置
            var dbOptions = app.ApplicationServices.GetService<DbOptions>();
            var dbModuleOptions = dbOptions.Modules.First(m => m.Name.EqualsIgnoreCase("Quartz"));
            var configProvider = app.ApplicationServices.GetService<IConfigProvider>();
            var quartzConfig = configProvider.Get<QuartzConfig>();
            quartzConfig.Provider = SqlDialect2QuartzProvider(dbOptions.Dialect);
            quartzConfig.ConnectionString = dbModuleOptions.ConnectionString;
            configProvider.Set(ConfigType.Library, "Quartz", JsonConvert.SerializeObject(quartzConfig));


            app.ApplicationServices.GetService<IQuartzServer>().Start();
        }

        /// <summary>
        /// 配置MVC功能
        /// </summary>
        /// <param name="mvcOptions"></param>
        public void ConfigureMvc(MvcOptions mvcOptions)
        {

        }

        protected QuartzProvider SqlDialect2QuartzProvider(SqlDialect sqlDialect)
        {
            switch (sqlDialect)
            {
                case SqlDialect.MySql: return QuartzProvider.MySql;
                case SqlDialect.SQLite: return QuartzProvider.SQLite;
                case SqlDialect.PostgreSQL: return QuartzProvider.PostgreSQL;
                case SqlDialect.Oracle: return QuartzProvider.Oracle;
                default: return QuartzProvider.SqlServer;
            }
        }
    }
}
