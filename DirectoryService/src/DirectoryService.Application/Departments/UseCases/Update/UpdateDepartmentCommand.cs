using DirectoryService.Contracts.Departments.Requests;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Departments.UseCases.Update;

public record UpdateDepartmentCommand(Guid Id, UpdateDepartmentRequest Request) : ICommand;