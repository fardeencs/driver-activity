using DriverActivityWeb.Contracts;
using DriverActivityWeb.Helper;
using DriverActivityWeb.ViewModels;
using FluentValidation;

namespace DriverActivityWeb.Validator
{
    public class AuthenticateRequestValidator : AbstractValidator<AuthenticateRequest>
    {
        private readonly IAppUserService appUserService;

        public AuthenticateRequestValidator(IAppUserService appUserService)
        {
            this.appUserService = appUserService;

            RuleFor(x => x.Username)
                        .NotNull().WithMessage("User name".RequiredMsg())
                        .NotEmpty().WithMessage("User name".RequiredMsg())
                        .Must(ValidateUserName).WithMessage("User name not found.");

            RuleFor(x => x.Password)
                .NotNull().WithMessage("Password".RequiredMsg())
                .NotEmpty().WithMessage("Password".RequiredMsg())
                .Must(ValidateUser).WithMessage("User not found.");
        }

        private bool ValidateUserName(string userName)
        {
            return this.appUserService.IsValidUser(userName);
        }


        private bool ValidateUser(AuthenticateRequest req, string? password)
        {
            return this.appUserService.IsValidUser(req.Username, password);
        }
    }
}
