using FluentValidation;

namespace ProductSystem.BLL.Validators
{
    public class RegisterDtoValidator : AbstractValidator<DTOs.RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        }
    }

    public class LoginDtoValidator : AbstractValidator<DTOs.LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
