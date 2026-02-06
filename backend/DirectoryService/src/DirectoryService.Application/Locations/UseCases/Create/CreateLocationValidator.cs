using DirectoryService.Domain.Locations.ValueObjects;
using FluentValidation;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Locations.UseCases.Create;

public class CreateLocationValidator : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithError(GeneralErrors.ValueIsRequired("request"));

        RuleFor(x => x.Request.Name).MustBeValueObject(LocationName.Of);

        RuleFor(x => x.Request.Timezone).MustBeValueObject(Timezone.Of);

        RuleFor(x => x.Request.Address).MustBeValueObject(Address.Of);
    }
}