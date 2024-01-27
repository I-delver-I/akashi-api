using AkaShi.Core.DTO.Auth;
using FluentValidation;

namespace AkaShi.WebAPI.Validators;

public sealed class RevokeRefreshTokenDTOValidator : AbstractValidator<RevokeRefreshTokenDTO>
{
    public RevokeRefreshTokenDTOValidator()
    {
        RuleFor(r => r.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token can not be empty.");
    }
}