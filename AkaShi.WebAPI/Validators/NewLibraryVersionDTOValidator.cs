using AkaShi.Core.DTO.LibraryVersion;
using FluentValidation;

namespace AkaShi.WebAPI.Validators;

public sealed class NewLibraryVersionDTOValidator : AbstractValidator<NewLibraryVersionDTO>
{
    public NewLibraryVersionDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name can not be empty.");

        RuleFor(x => x.Name)
            .Matches(@"^\d+\.\d+\.\d+$")
            .WithMessage("The library version name must be in the format number.number.number (e.g., 15.2.18)");

        RuleFor(x => x.LibraryId)
            .NotEmpty()
            .WithMessage("LibraryId can not be empty.");

        RuleFor(x => x.LibraryVersionArchive)
            .NotEmpty()
            .WithMessage("LibraryVersionArchive can not be empty.");
    }
}