using DirectoryService.Domain.Positions.ValueObjects;
using FluentValidation;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Positions.UseCases.Create;

public class CreatePositionValidator : AbstractValidator<CreatePositionCommand>
{
    public CreatePositionValidator()
    {
        RuleFor(x => x.Request.Name).MustBeValueObject(PositionName.Of);

        RuleFor(x => x.Request.Description).MustBeValueObject(Description.Of);

        RuleFor(x => x.Request.DepartmentIds)
            .Must(departmentIds => departmentIds.Distinct().Count() == departmentIds.Length).WithError(GeneralErrors.ArrayContainsDuplicates("position.departmentIds"))
            .NotEmpty().WithError(GeneralErrors.ArrayIsRequired("position.departmentIds"));

        RuleForEach(x => x.Request.DepartmentIds)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("position.departmentIds"));
    }
}