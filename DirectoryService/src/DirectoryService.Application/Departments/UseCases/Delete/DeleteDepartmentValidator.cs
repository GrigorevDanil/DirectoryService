using FluentValidation;
using Shared;
using Shared.Validation;

namespace DirectoryService.Application.Departments.UseCases.Delete;

public class DeleteDepartmentValidator : AbstractValidator<DeleteDepartmentCommand>
{
    public DeleteDepartmentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("department.id"));
    }
}