using FluentValidation;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Locations.UseCases.Delete;

public class DeleteLocationValidator : AbstractValidator<DeleteLocationCommand>
{
    public DeleteLocationValidator()
    {
        RuleFor(x => x.LocationId)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("location.id"));
    }
}