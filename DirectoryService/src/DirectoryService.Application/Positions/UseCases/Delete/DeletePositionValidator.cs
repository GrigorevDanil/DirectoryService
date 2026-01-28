using FluentValidation;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Positions.UseCases.Delete;

public class DeletePositionValidator : AbstractValidator<DeletePositionCommand>
{
    public DeletePositionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("position.id"));
    }
}