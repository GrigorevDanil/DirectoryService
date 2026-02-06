using DirectoryService.Contracts.Departments.Requests;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Departments.UseCases.AttachVideo;

public record AttachVideoForDepartmentCommand(Guid DepartmentId, AttachVideoForDepartmentRequest Request) : ICommand;