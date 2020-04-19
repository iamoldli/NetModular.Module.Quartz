using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetModular.Lib.Auth.Web.Attributes;
using NetModular.Lib.Quartz.Abstractions;
using NetModular.Lib.Utils.Core.Extensions;
using NetModular.Module.Quartz.Application.JobService;
using NetModular.Module.Quartz.Application.JobService.ViewModels;
using NetModular.Module.Quartz.Domain.Job.Models;
using NetModular.Module.Quartz.Domain.JobHttp;
using NetModular.Module.Quartz.Domain.JobLog.Models;

namespace NetModular.Module.Quartz.Web.Controllers
{
    [Description("任务管理")]
    public class JobController : ModuleController
    {
        private readonly IJobService _service;
        private readonly IQuartzModuleCollection _moduleCollection;

        public JobController(IJobService service, IQuartzModuleCollection moduleCollection)
        {
            _service = service;
            _moduleCollection = moduleCollection;
        }

        [HttpGet]
        [Description("查询")]
        public Task<IResultModel> Query([FromQuery]JobQueryModel model)
        {
            return _service.Query(model);
        }

        [HttpPost]
        [Description("添加")]
        public Task<IResultModel> Add(JobAddModel model)
        {
            return _service.Add(model);
        }

        [HttpGet]
        [Description("编辑")]
        public Task<IResultModel> Edit([BindRequired]Guid id)
        {
            return _service.Edit(id);
        }

        [HttpPost]
        [Description("修改")]
        public Task<IResultModel> Update(JobUpdateModel model)
        {
            return _service.Update(model);
        }

        [HttpDelete]
        [Description("删除")]
        public Task<IResultModel> Delete([BindRequired]Guid id)
        {
            return _service.Delete(id);
        }

        [HttpPost]
        [Description("暂停")]
        public Task<IResultModel> Pause([BindRequired]Guid id)
        {
            return _service.Pause(id);
        }

        [HttpPost]
        [Description("回复")]
        public Task<IResultModel> Resume([BindRequired]Guid id)
        {
            return _service.Resume(id);
        }

        [HttpPost]
        [Description("停止")]
        public Task<IResultModel> Stop([BindRequired]Guid id)
        {
            return _service.Stop(id);
        }

        [HttpGet]
        [Description("日志")]
        public Task<IResultModel> Log([FromQuery]JobLogQueryModel model)
        {
            return _service.Log(model);
        }

        [HttpGet]
        [Common]
        public IResultModel JobSelect(string moduleCode)
        {
            var module = _moduleCollection.FirstOrDefault(m => m.Module.Code == moduleCode);
            if (module == null)
                return ResultModel.Failed();

            return ResultModel.Success(module.TaskSelect);
        }

        [HttpPost]
        [Description("添加HTTP任务")]
        public Task<IResultModel> AddHttpJob(JobHttpAddModel model)
        {
            return _service.AddHttpJob(model);
        }

        [HttpGet]
        [Description("编辑HTTP任务")]
        public Task<IResultModel> EditHttpJob([BindRequired]Guid id)
        {
            return _service.EditHttpJob(id);
        }

        [HttpPost]
        [Description("修改HTTP任务")]
        public Task<IResultModel> UpdateHttpJob(JobHttpUpdateModel model)
        {
            return _service.UpdateHttpJob(model);
        }

        [HttpGet]
        [Common]
        public IResultModel AuthTypeSelect()
        {
            return ResultModel.Success(EnumExtensions.ToResult<AuthType>());
        }

        [HttpGet]
        [Common]
        public IResultModel ContentTypeSelect()
        {
            return ResultModel.Success(EnumExtensions.ToResult<ContentType>());
        }

        [HttpGet]
        [Description("HTTP任务详情")]
        public Task<IResultModel> JobHttpDetails(Guid id)
        {
            return _service.JobHttpDetails(id);
        }
    }
}
