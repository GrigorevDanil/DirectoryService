using FluentValidation;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Departments.UseCases.SetLocations;

public class SetLocationForDepartmentValidator : AbstractValidator<SetLocationsForDepartmentCommand>
{
    public SetLocationForDepartmentValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("department.departmentId"));

        RuleFor(x => x.Request.LocationIds)
            .Must(locationIds => locationIds.Distinct().Count() == locationIds.Length).WithError(GeneralErrors.ArrayContainsDuplicates("department.locationIds"))
            .NotEmpty().WithError(GeneralErrors.ArrayIsRequired("department.locationIds"));

        RuleForEach(x => x.Request.LocationIds)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("department.locationIds"));
    }
}