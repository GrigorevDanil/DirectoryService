using DirectoryService.Domain.Locations.ValueObjects;
using FluentValidation;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Locations.UseCases.Update;

public class UpdateLocationValidator : AbstractValidator<UpdateLocationCommand>
{
    public UpdateLocationValidator()
    {
        RuleFor(x => x.LocationId)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("location.id"));

        RuleFor(x => x.Request.Name).MustBeValueObject(LocationName.Of);

        RuleFor(x => x.Request.Timezone).MustBeValueObject(Timezone.Of);

        RuleFor(x => x.Request.Address).MustBeValueObject(Address.Of);
    }
}