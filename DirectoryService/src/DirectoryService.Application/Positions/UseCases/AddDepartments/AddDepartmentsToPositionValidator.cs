using FluentValidation;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Positions.UseCases.AddDepartment;

public class AddDepartmentsToPositionValidator : AbstractValidator<AddDepartmentsToPositionCommand>
{
    public AddDepartmentsToPositionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("position.id"));

        RuleForEach(x => x.Request.DepartmentIds)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("position.departmentId"));
    }
}