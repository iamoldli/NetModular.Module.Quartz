using System.Collections.Specialized;
using System.Linq;
using NetModular.Lib.Data.Abstractions.Enums;
using NetModular.Lib.Data.Abstractions.Options;
using NetModular.Lib.Options.Abstraction;
using NetModular.Lib.Utils.Core.Extensions;
using NetModular.Module.Quartz.Infrastructure.Core;

namespace NetModular.Module.Quartz.Infrastructure
{
    /// <summary>
    /// 任务调度配置项
    /// </summary>
    public class QuartzOptions : IModuleOptions
    {
        #region ==属性==

        /// <summary>
        /// 开启
        /// </summary>
        [ModuleOptionDefinition("开启")]
        public bool Enabled { get; set; }

        /// <summary>
        /// 启用日志
        /// </summary>
        [ModuleOptionDefinition("启用日志")]
        public bool EnabledLogger { get; set; } = true;

        /// <summary>
        /// 实例名称
        /// </summary>
        [ModuleOptionDefinition("实例名称")]
        public string InstanceName { get; set; } = "QuartzServer";

        /// <summary>
        /// 表前缀
        /// </summary>
        [ModuleOptionDefinition("表前缀")]
        public string TablePrefix { get; set; } = "QRTZ_";

        /// <summary>
        /// 序列化方式
        /// </summary>
        [ModuleOptionDefinition("序列化方式")]
        public QuartzSerializerType SerializerType { get; set; } = QuartzSerializerType.Json;

        /// <summary>
        /// 数据库
        /// </summary>
        [ModuleOptionDefinition("数据库")]
        public string Provider { get; set; }

        #endregion

        private readonly DbOptions _dbOptions;

        public QuartzOptions(DbOptions dbOptions)
        {
            _dbOptions = dbOptions;
        }

        /// <summary>
        /// 获取任务调度服务所需的属性
        /// </summary>
        /// <returns></returns>
        public NameValueCollection GetQuartzProps()
        {
            var quartzProps = new NameValueCollection();
            var quartzDbOptions = _dbOptions.Modules.FirstOrDefault(m => m.Name.EqualsIgnoreCase("quartz"));
            if (quartzDbOptions != null)
            {
                quartzProps["quartz.scheduler.instanceName"] = InstanceName;
                quartzProps["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX,Quartz";
                quartzProps["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.StdAdoDelegate,Quartz";
                quartzProps["quartz.jobStore.tablePrefix"] = TablePrefix;
                quartzProps["quartz.jobStore.dataSource"] = "default";
                quartzProps["quartz.dataSource.default.connectionString"] = quartzDbOptions.ConnectionString;
                quartzProps["quartz.dataSource.default.provider"] = Provider.IsNull() ? GetProvider(_dbOptions.Dialect) : Provider;
                quartzProps["quartz.serializer.type"] = SerializerType.ToDescription();
            }

            return quartzProps;
        }

        private string GetProvider(SqlDialect dialect)
        {
            switch (dialect)
            {
                case SqlDialect.SqlServer:
                    return "SqlServer";
                case SqlDialect.MySql:
                    return "MySql";
                case SqlDialect.SQLite:
                    return "SQLite-Microsoft";
                case SqlDialect.Oracle:
                    return "OracleODP";
                case SqlDialect.PostgreSQL:
                    return "Npgsql";
            }

            return "";
        }

        public IModuleOptions Copy()
        {
            return new QuartzOptions(_dbOptions)
            {
                Enabled = Enabled,
                EnabledLogger = EnabledLogger,
                InstanceName = InstanceName,
                Provider = Provider,
                SerializerType = SerializerType,
                TablePrefix = TablePrefix
            };
        }
    }
}
