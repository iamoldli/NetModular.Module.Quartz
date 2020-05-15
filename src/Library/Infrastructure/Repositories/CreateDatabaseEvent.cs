using System.Threading.Tasks;
using Dapper;
using NetModular.Lib.Data.Abstractions;
using NetModular.Lib.Data.Abstractions.Enums;

namespace NetModular.Module.Quartz.Infrastructure.Repositories
{
    public class CreateDatabaseEvent : IDatabaseCreateEvents
    {
        public IDbContext DbContext { get; set; }

        public Task Before()
        {
            return Task.CompletedTask;
        }


        public async Task After()
        {
            var sql = new CreateTableSql();
            bool exist;
            using var con = DbContext.NewConnection();
            switch (DbContext.Options.DbOptions.Dialect)
            {
                case SqlDialect.SqlServer:
                    exist = con.ExecuteScalar<int>("SELECT TOP 1 1 FROM sysobjects WHERE id = OBJECT_ID(N'QRTZ_BLOB_TRIGGERS') AND xtype = 'U';") > 0;
                    if (!exist)
                    {
                        foreach (var cmd in sql.SqlServer)
                        {
                            await con.ExecuteAsync(cmd);
                        }
                    }
                    break;
                case SqlDialect.MySql:
                    exist = con.ExecuteScalar<int>($"SELECT 1 FROM information_schema.TABLES WHERE table_schema = '{DbContext.Options.DbModuleOptions.Database}' AND table_name = 'qrtz_blob_triggers' limit 1;") > 0;
                    if (!exist)
                    {
                        foreach (var cmd in sql.MySql)
                        {
                            await con.ExecuteAsync(cmd);
                        }
                    }
                    break;
                case SqlDialect.SQLite:
                    exist = con.ExecuteScalar<int>("SELECT 1 FROM sqlite_master WHERE type = 'table' and name='QRTZ_BLOB_TRIGGERS';") > 0;
                    if (!exist)
                    {
                        foreach (var cmd in sql.SQLite)
                        {
                            await con.ExecuteAsync(cmd);
                        }
                    }
                    break;
                case SqlDialect.PostgreSQL:
                    exist = con.ExecuteScalar<int>("SELECT 1 FROM pg_namespace WHERE nspname = 'qrtz_triggers' LIMIT 1;") > 0;
                    if (!exist)
                    {
                        foreach (var cmd in sql.PostgreSQL)
                        {
                            await con.ExecuteAsync(cmd);
                        }
                    }
                    break;
            }

        }
    }
}
