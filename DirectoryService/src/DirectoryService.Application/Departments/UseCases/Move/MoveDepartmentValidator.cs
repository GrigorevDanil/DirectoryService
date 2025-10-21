using FluentValidation;
using Shared;
using Shared.Validation;

namespace DirectoryService.Application.Departments.UseCases.Move;

public class MoveDepartmentValidator : AbstractValidator<MoveDepartmentCommand>
{
    public MoveDepartmentValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("department.id"));

        RuleFor(x => x.Request.ParentId)
            .Must(parentId => parentId == null || parentId.Value != Guid.Empty).WithError(GeneralErrors.ValueIsRequired("department.parentId"))
            .Must((x, parentId ) => parentId != x.DepartmentId).WithError(GeneralErrors.ValueIsInvalid("parentId is the same as departmentId", "department.parentId"));
    }
}