using FluentValidation;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Departments.UseCases.AttachVideo;

public class AttachVideoForDepartmentValidator : AbstractValidator<AttachVideoForDepartmentCommand>
{
    public AttachVideoForDepartmentValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("department.departmentId"));

        RuleFor(x => x.Request.VideoId)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired("department.videoId"));
    }
}