using AkaShi.Core.DTO.Library;
using FluentValidation;

namespace AkaShi.WebAPI.Validators;

public class NewLibraryDTOValidator : AbstractValidator<NewLibraryDTO>
{
    public NewLibraryDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name can not be empty.");
        
        RuleFor(x => x.InitialVersionName)
            .NotEmpty()
            .WithMessage("Name can not be empty.");

        RuleFor(x => x.InitialVersionName)
            .Matches(@"^\d+\.\d+\.\d+$")
            .WithMessage("The library version name must be in the format number.number.number (e.g., 15.2.18)");
        
        RuleFor(x => x.InitialVersionArchive)
            .NotEmpty()
            .WithMessage("LibraryVersionArchive can not be empty.");
    }
}