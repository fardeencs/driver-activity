using DriverActivityWeb.Contracts;
using DriverActivityWeb.Helper;
using DriverActivityWeb.ViewModels;
using FluentValidation;

namespace DriverActivityWeb.Validator
{
    public class RegisterRequestValidator :  AbstractValidator<RegisterRequest>
    {
        private readonly IAppUserService appUserService;

        public RegisterRequestValidator(IAppUserService appUserService)
        {
            this.appUserService = appUserService;

            RuleFor(x => x.UserName)
                        .NotNull().WithMessage("User name".RequiredMsg())
                        .NotEmpty().WithMessage("User name".RequiredMsg())
                        .Must(IsUserNameExist).WithMessage("Use another User Name.");

            RuleFor(x => x.StaffId)
                        .NotNull().WithMessage("Staff ID".RequiredMsg())
                        .NotEmpty().WithMessage("Staff ID".RequiredMsg())
                        .Must(IsStaffIDExist).WithMessage("Staff ID already exist.");

            RuleFor(x => x.Password)
                        .NotNull().WithMessage("Password".RequiredMsg())
                        .NotEmpty().WithMessage("Password".RequiredMsg());

            RuleFor(x => x.NameEn)
                        .NotNull().WithMessage("Full Name".RequiredMsg())
                        .NotEmpty().WithMessage("Full Name".RequiredMsg());
        }

        private bool IsUserNameExist(string? userName)
        {
            if (AppUtility.IsEmpty(userName))
                return false;

            return !this.appUserService.IsUserNameExsit(userName);
        }

        private bool IsStaffIDExist(string? staffId)
        {
            if (AppUtility.IsEmpty(staffId))
                return false;

            int staffID = Convert.ToInt32(staffId);
            if (staffID <= 0)
                return false;

            return !this.appUserService.IsStaffIDExist(staffID);
        }
    }
}
