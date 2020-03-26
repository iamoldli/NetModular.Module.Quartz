using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetModular.Lib.Auth.Web.Attributes;
using NetModular.Module.Quartz.Application.GroupService;
using NetModular.Module.Quartz.Application.GroupService.ViewModels;
using NetModular.Module.Quartz.Domain.Group.Models;

namespace NetModular.Module.Quartz.Web.Controllers
{
    [Description("任务组管理")]
    public class GroupController : ModuleController
    {
        private readonly IGroupService _service;

        public GroupController(IGroupService service)
        {
            _service = service;
        }

        [HttpGet]
        [Description("查询")]
        public Task<IResultModel> Query([FromQuery]GroupQueryModel model)
        {
            return _service.Query(model);
        }

        [HttpPost]
        [Description("添加")]
        public Task<IResultModel> Add(GroupAddModel model)
        {
            return _service.Add(model);
        }

        [HttpDelete]
        [Description("删除")]
        public Task<IResultModel> Delete([BindRequired]Guid id)
        {
            return _service.Delete(id);
        }

        /// <summary>
        /// 下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Common]
        public Task<IResultModel> Select()
        {
            return _service.Select();
        }
    }
}
