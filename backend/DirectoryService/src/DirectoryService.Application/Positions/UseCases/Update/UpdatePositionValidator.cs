using DirectoryService.Domain.Positions.ValueObjects;
using FluentValidation;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Positions.UseCases.Update;

public class UpdatePositionValidator : AbstractValidator<UpdatePositionCommand>
{
    public UpdatePositionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("position.id"));

        RuleFor(x => x.Request.Name).MustBeValueObject(PositionName.Of);

        RuleFor(x => x.Request.Description).MustBeValueObject(Description.Of);
    }
}