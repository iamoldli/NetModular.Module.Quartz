using System;
using System.Threading.Tasks;
using NetModular.Lib.Config.Abstractions;
using NetModular.Lib.Quartz.Abstractions;
using NetModular.Module.Quartz.Domain.JobLog;

namespace NetModular.Module.Quartz.Infrastructure.Core
{
    /// <summary>
    /// 任务日志
    /// </summary>
    public class TaskLogger : ITaskLogger
    {
        private readonly IJobLogRepository _repository;
        private readonly IConfigProvider _configProvider;

        public TaskLogger(IJobLogRepository repository, IConfigProvider configProvider)
        {
            _repository = repository;
            _configProvider = configProvider;
        }

        public Guid JobId { get; set; }

        public async Task Info(string msg)
        {
            var config = _configProvider.Get<QuartzConfig>();
            if (!config.Logger)
                return;

            var entity = new JobLogEntity
            {
                JobId = JobId,
                Type = JobLogType.Info,
                Msg = msg,
                CreatedTime = DateTime.Now
            };

            await _repository.AddAsync(entity);
        }

        public async Task Debug(string msg)
        {
            var config = _configProvider.Get<QuartzConfig>();
            if (!config.Logger)
                return;

            var entity = new JobLogEntity
            {
                JobId = JobId,
                Type = JobLogType.Debug,
                Msg = msg,
                CreatedTime = DateTime.Now
            };

            await _repository.AddAsync(entity);
        }

        public async Task Error(string msg)
        {
            var config = _configProvider.Get<QuartzConfig>();
            if (!config.Logger)
                return;

            var entity = new JobLogEntity
            {
                JobId = JobId,
                Type = JobLogType.Error,
                Msg = msg,
                CreatedTime = DateTime.Now
            };

            await _repository.AddAsync(entity);
        }
    }
}
