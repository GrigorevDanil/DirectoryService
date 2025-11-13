using FluentValidation;
using Shared;
using Shared.Validation;

namespace DirectoryService.Application.Departments.UseCases.SetLocations;

public class SetLocationForDepartmentValidator : AbstractValidator<SetLocationsForDepartmentCommand>
{
    public SetLocationForDepartmentValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("department.departmentId"));

        RuleFor(x => x.ForDepartmentRequest.LocationIds)
            .Must(locationIds => locationIds.Distinct().Count() == locationIds.Length).WithError(GeneralErrors.ArrayContainsDuplicates("department.locationIds"))
            .NotEmpty().WithError(GeneralErrors.ArrayIsRequired("department.locationIds"));

        RuleForEach(x => x.ForDepartmentRequest.LocationIds)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("department.locationIds"));
    }
}