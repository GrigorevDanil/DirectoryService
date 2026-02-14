using DirectoryService.Contracts.Departments.Requests;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Departments.UseCases.Create;

public record CreateDepartmentCommand(CreateDepartmentRequest Request) : ICommand;