using AkaShi.Core.DTO.User;
using FluentValidation;

namespace AkaShi.WebAPI.Validators;

public sealed class UserRegisterDTOValidator : AbstractValidator<UserRegisterDTO>
{
    public UserRegisterDTOValidator()
    {
        RuleFor(u => u.Username)
            .NotEmpty()
            .WithMessage("Username is mandatory.")
            .Length(3, 50)
            .WithMessage("Username should be minimum 3 character and maximum 50.");

        RuleFor(u => u.Email)
            .EmailAddress();

        RuleFor(u => u.Password)
            .Length(4, 16)
            .WithMessage("Password must be from 4 to 16 characters.");
    }
}