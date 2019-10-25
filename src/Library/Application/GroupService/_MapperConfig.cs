using AutoMapper;
using NetModular.Lib.Mapper.AutoMapper;
using NetModular.Module.Quartz.Application.GroupService.ViewModels;
using NetModular.Module.Quartz.Domain.Group;

namespace NetModular.Module.Quartz.Application.GroupService
{
    public class MapperConfig : IMapperConfig
    {
        public void Bind(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<GroupAddModel, GroupEntity>();
        }
    }
}
