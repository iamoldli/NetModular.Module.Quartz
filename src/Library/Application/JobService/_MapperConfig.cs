using AutoMapper;
using NetModular.Lib.Mapper.AutoMapper;
using NetModular.Module.Quartz.Application.JobService.ViewModels;
using NetModular.Module.Quartz.Domain.Job;

namespace NetModular.Module.Quartz.Application.JobService
{
    public class MapperConfig : IMapperConfig
    {
        public void Bind(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<JobAddModel, JobEntity>();
            cfg.CreateMap<JobEntity, JobUpdateModel>();
            cfg.CreateMap<JobUpdateModel, JobEntity>();
        }
    }
}
