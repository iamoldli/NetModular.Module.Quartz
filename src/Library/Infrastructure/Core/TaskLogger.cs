using System;
using System.Threading.Tasks;
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
        private readonly QuartzOptions _options;

        public TaskLogger(IJobLogRepository repository, QuartzOptions options)
        {
            _repository = repository;
            _options = options;
        }

        public Guid JobId { get; set; }

        public async Task Info(string msg)
        {
            if (!_options.EnabledLogger)
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
            if (!_options.EnabledLogger)
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
            if (!_options.EnabledLogger)
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
