using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NetModular.Lib.Quartz.Abstractions;
using NetModular.Lib.Utils.Core.Result;
using NetModular.Module.Quartz.Application.JobService.ViewModels;
using NetModular.Module.Quartz.Domain.Job;
using NetModular.Module.Quartz.Domain.Job.Models;
using NetModular.Module.Quartz.Domain.JobLog;
using NetModular.Module.Quartz.Domain.JobLog.Models;
using NetModular.Module.Quartz.Infrastructure.Repositories;
using Quartz;

namespace NetModular.Module.Quartz.Application.JobService
{
    public class JobService : IJobService
    {
        private readonly IMapper _mapper;
        private readonly IJobRepository _repository;
        private readonly IJobLogRepository _logRepository;
        private readonly ILogger _logger;
        private readonly IQuartzServer _quartzServer;
        private readonly QuartzDbContext _dbContext;

        public JobService(IMapper mapper, IJobRepository repository, ILogger<JobService> logger, IQuartzServer quartzServer, IJobLogRepository logRepository, QuartzDbContext dbContext)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
            _quartzServer = quartzServer;
            _logRepository = logRepository;
            _dbContext = dbContext;
        }

        public async Task<IResultModel> Query(JobQueryModel model)
        {
            var result = new QueryResultModel<JobEntity>
            {
                Rows = await _repository.Query(model),
                Total = model.TotalCount
            };
            return ResultModel.Success(result);
        }

        public async Task<IResultModel> Add(JobAddModel model)
        {
            var entity = _mapper.Map<JobEntity>(model);
            entity.JobKey = $"{model.Group}.{model.Code}";
            entity.Status = JobStatus.Pause;//Ĭ�����Ӻ���ͣ����Ҫ�ֶ�����
            entity.EndDate = entity.EndDate.AddDays(1);

            if (await _repository.Exists(entity))
            {
                return ResultModel.Failed($"��ǰ������{entity.Group}�Ѵ����������${entity.Code}");
            }

            using (var uow = _dbContext.NewUnitOfWork())
            {
                if (await _repository.AddAsync(entity, uow))
                {
                    var result = await AddJob(entity);
                    if (!result.Successful)
                    {
                        return result;
                    }

                    uow.Commit();

                    return ResultModel.Success();
                }
            }

            return ResultModel.Failed("����ʧ��");
        }

        public async Task<IResultModel> Edit(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            if (entity == null)
            {
                return ResultModel.Failed("���񲻴���");
            }

            var model = _mapper.Map<JobUpdateModel>(entity);
            model.EndDate = entity.EndDate.AddDays(-1);

            return ResultModel.Success(model);
        }

        public async Task<IResultModel> Update(JobUpdateModel model)
        {
            var entity = await _repository.GetAsync(model.Id);
            if (entity == null)
            {
                return ResultModel.Failed("���񲻴���");
            }

            var oldStatus = entity.Status;
            var oldJobKey = new JobKey(entity.Name, entity.Group);

            _mapper.Map(model, entity);
            entity.JobKey = $"{model.Group}.{model.Code}";
            entity.EndDate = entity.EndDate.AddDays(1);

            if (await _repository.Exists(entity))
            {
                return ResultModel.Failed($"��ǰ������{entity.Group}�Ѵ����������${entity.Code}");
            }

            using (var uow = _dbContext.NewUnitOfWork())
            {
                //δ����״̬�޸�Ϊ��ͣ
                if (oldStatus != JobStatus.Running)
                {
                    entity.Status = JobStatus.Pause;
                }

                //�������δ��ɵ�������Ҫ�ȴӵ��ȷ�����ɾ������
                if (oldStatus != JobStatus.Completed)
                {
                    await _quartzServer.DeleteJob(oldJobKey);
                }

                if (await _repository.UpdateAsync(entity, uow))
                {
                    var result = await AddJob(entity, entity.Status == JobStatus.Running);
                    if (!result.Successful)
                    {
                        return result;
                    }

                    uow.Commit();
                    return ResultModel.Success();
                }
            }

            return ResultModel.Failed("����ʧ��");
        }

        public async Task<IResultModel> Delete(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            if (entity == null)
            {
                return ResultModel.Failed("���񲻴���");
            }

            var result = await _repository.DeleteAsync(id);
            if (result)
            {
                if (entity.Status != JobStatus.Completed)
                {
                    var jobKey = new JobKey(entity.Code, entity.Group);

                    await _quartzServer.DeleteJob(jobKey);
                }

                return ResultModel.Success("��ɾ��");
            }

            return ResultModel.Failed("ɾ��ʧ��");
        }

        public async Task<IResultModel> Pause(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            if (entity == null)
            {
                return ResultModel.Failed("���񲻴���");
            }

            if (entity.Status == JobStatus.Completed)
            {
                return ResultModel.Failed("�����ѽ���");
            }

            if (entity.Status != JobStatus.Pause)
            {
                entity.Status = JobStatus.Pause;
                using (var uow = _dbContext.NewUnitOfWork())
                {
                    if (await _repository.UpdateAsync(entity))
                    {
                        try
                        {
                            var jobKey = new JobKey(entity.Code, entity.Group);

                            await _quartzServer.PauseJob(jobKey);

                            uow.Commit();

                            return ResultModel.Success("����ͣ");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"������ͣʧ�ܣ�{ex}");
                        }
                    }
                }
            }

            return ResultModel.Failed("��ͣʧ��");
        }

        public async Task<IResultModel> Resume(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            if (entity == null)
            {
                return ResultModel.Failed("���񲻴���");
            }

            if (entity.Status != JobStatus.Running)
            {
                var oldStatus = entity.Status;
                entity.Status = JobStatus.Running;
                using (var uow = _dbContext.NewUnitOfWork())
                {
                    if (await _repository.UpdateAsync(entity, uow))
                    {
                        try
                        {
                            //����ɵ�����������Ҫ���¼��뵽������
                            if (oldStatus == JobStatus.Completed)
                            {
                                if (entity.EndDate <= DateTime.Now)
                                {
                                    return ResultModel.Failed("����ʱЧ�ѹ���");
                                }

                                var result = await AddJob(entity, true);
                                if (!result.Successful)
                                {
                                    return result;
                                }
                            }
                            else
                            {
                                var jobKey = new JobKey(entity.Code, entity.Group);

                                await _quartzServer.ResumeJob(jobKey);
                            }

                            uow.Commit();

                            return ResultModel.Success("������");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"��������ʧ�ܣ�{ex}");
                        }
                    }
                }
            }

            return ResultModel.Failed("����ʧ��");
        }

        public async Task<IResultModel> Log(JobLogQueryModel model)
        {
            var result = new QueryResultModel<JobLogEntity>
            {
                Rows = await _logRepository.Query(model),
                Total = model.TotalCount
            };
            return ResultModel.Success(result);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="start">�Ƿ���������</param>
        /// <returns></returns>
        private async Task<IResultModel> AddJob(JobEntity entity, bool start = false)
        {
            var jobClassType = Type.GetType(entity.JobClass);
            if (jobClassType == null)
            {
                return ResultModel.Failed($"������({entity.JobClass})������");
            }

            var jobKey = new JobKey(entity.Code, entity.Group);
            var job = JobBuilder.Create(jobClassType).WithIdentity(jobKey).UsingJobData("id", entity.Id.ToString()).Build();
            var triggerBuilder = TriggerBuilder.Create().WithIdentity(entity.Code, entity.Group)
                .EndAt(entity.EndDate.ToUniversalTime())
                .WithDescription(entity.Name);

            //�����ʼ����С�ڵ��ڵ�ǰ��������������
            if (entity.BeginDate <= DateTime.Now)
            {
                triggerBuilder.StartNow();
            }
            else
            {
                triggerBuilder.StartAt(entity.BeginDate.ToUniversalTime());
            }

            if (entity.TriggerType == TriggerType.Simple)
            {
                //������
                triggerBuilder.WithSimpleSchedule(builder =>
                {
                    builder.WithIntervalInSeconds(entity.Interval);
                    if (entity.RepeatCount > 0)
                    {
                        builder.WithRepeatCount(entity.RepeatCount - 1);
                    }
                    else
                    {
                        builder.RepeatForever();
                    }
                });
            }
            else
            {
                if (!CronExpression.IsValidExpression(entity.Cron))
                {
                    return ResultModel.Failed("CRON����ʽ��Ч");
                }

                //CRON����
                triggerBuilder.WithCronSchedule(entity.Cron);
            }

            var trigger = triggerBuilder.Build();
            try
            {
                await _quartzServer.AddJob(job, trigger);

                if (!start)
                {
                    //����ͣ
                    await _quartzServer.PauseJob(jobKey);
                }

                return ResultModel.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError("���������������ʧ��" + ex);
            }

            return ResultModel.Failed();
        }
    }
}
