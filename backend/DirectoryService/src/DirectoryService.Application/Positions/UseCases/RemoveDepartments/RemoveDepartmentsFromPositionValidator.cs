using FluentValidation;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Positions.UseCases.RemoveDepartments;

public class RemoveDepartmentsFromPositionValidator : AbstractValidator<RemoveDepartmentsFromPositionCommand>
{
    public RemoveDepartmentsFromPositionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("position.id"));

        RuleForEach(x => x.Request.DepartmentIds)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("position.departmentId"));
    }
}