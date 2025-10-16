using DirectoryService.Domain.Departments.ValueObjects;
using FluentValidation;
using Shared;
using Shared.Validation;

namespace DirectoryService.Application.Departments.UseCases.Create;

public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentValidator()
    {
        RuleFor(x => x.Request.Name).MustBeValueObject(DepartmentName.Of);

        RuleFor(x => x.Request.Identifier).MustBeValueObject(Identifier.Of);

        RuleFor(x => x.Request.ParentId)
            .Must(parentId => parentId == null || parentId.Value != Guid.Empty).WithError(GeneralErrors.ValueIsRequired("department.parentId"));

        RuleFor(x => x.Request.LocationIds)
            .Must(locationIds => locationIds.Distinct().Count() == locationIds.Length).WithError(GeneralErrors.ArrayContainsDuplicates("department.locationIds"))
            .NotEmpty().WithError(GeneralErrors.ArrayIsRequired("department.locationIds"));

        RuleForEach(x => x.Request.LocationIds)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("department.locationIds"));
    }
}