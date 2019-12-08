using FluentValidation;
using NetModular.Lib.Validation.FluentValidation;
using NetModular.Module.Quartz.Application.JobService.ViewModels;

namespace NetModular.Module.Quartz.Web.Validators
{
    public class JobHttpAddValidator : JobBaseValidator<JobHttpAddModel>
    {
        public JobHttpAddValidator()
        {
            RuleFor(x => x.Token).Required().When(x => x.AuthType == Domain.JobHttp.AuthType.Jwt).WithMessage("Jwt认证需要设置token");
        }
    }
}
