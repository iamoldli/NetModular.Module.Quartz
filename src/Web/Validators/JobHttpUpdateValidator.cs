using FluentValidation;
using NetModular.Lib.Validation.FluentValidation;
using NetModular.Module.Quartz.Application.JobService.ViewModels;

namespace NetModular.Module.Quartz.Web.Validators
{
    public class JobHttpUpdateValidator : JobBaseValidator<JobHttpAddModel>
    {
        public JobHttpUpdateValidator()
        {
            RuleFor(x => x.Token).Required().When(x => x.AuthType == Domain.JobHttp.AuthType.Jwt).WithMessage("Jwt认证需要设置token");
        }
    }
}
