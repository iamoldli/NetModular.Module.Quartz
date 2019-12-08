using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NetModular.Lib.Quartz.Abstractions;
using NetModular.Module.Quartz.Domain.JobLog;
using NetModular.Module.Quartz.Infrastructure.Options;

namespace NetModular.Module.Quartz.Web.Core
{
    /// <summary>
    /// 任务日志
    /// </summary>
    public class JobLogger : IJobLogger
    {
        private readonly IJobLogRepository _repository;
        private readonly QuartzOptions _options;

        public JobLogger(IJobLogRepository repository, IOptionsMonitor<QuartzOptions> optionsMonitor)
        {
            _repository = repository;
            _options = optionsMonitor.CurrentValue;
        }

        public Guid JobId { get; set; }

        public async Task Info(string msg)
        {
            if (_options.DisabledLogger)
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
            if (_options.DisabledLogger)
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
            if (_options.DisabledLogger)
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
