using AkaShi.Core.DTO.User;
using FluentValidation;

namespace AkaShi.WebAPI.Validators;

public sealed class UserLoginDTOValidator : AbstractValidator<UserLoginDTO>
{
    public UserLoginDTOValidator()
    {
        RuleFor(u => u.Username).NotNull();
        RuleFor(u => u.Password).NotNull();
    }
}