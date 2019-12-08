using AutoMapper;
using NetModular.Lib.Mapper.AutoMapper;
using NetModular.Module.Quartz.Application.JobService.ViewModels;
using NetModular.Module.Quartz.Domain.Job;
using NetModular.Module.Quartz.Domain.JobHttp;

namespace NetModular.Module.Quartz.Application.JobService
{
    public class MapperConfig : IMapperConfig
    {
        public void Bind(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<JobAddModel, JobEntity>();
            cfg.CreateMap<JobEntity, JobUpdateModel>();
            cfg.CreateMap<JobUpdateModel, JobEntity>();
            cfg.CreateMap<JobHttpAddModel, JobEntity>();
            cfg.CreateMap<JobHttpAddModel, JobHttpEntity>();
            cfg.CreateMap<JobEntity, JobHttpUpdateModel>();
        }
    }
}
