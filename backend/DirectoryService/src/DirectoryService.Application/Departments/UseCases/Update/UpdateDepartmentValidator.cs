using DirectoryService.Domain.Departments.ValueObjects;
using FluentValidation;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Departments.UseCases.Update;

public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("department.id"));

        RuleFor(x => x.Request.Name).MustBeValueObject(DepartmentName.Of);

        RuleFor(x => x.Request.Identifier).MustBeValueObject(Identifier.Of);
    }
}