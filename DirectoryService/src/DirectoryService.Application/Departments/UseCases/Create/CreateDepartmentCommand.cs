using DirectoryService.Contracts.Departments.Requests;
using Shared.Abstractions;

namespace DirectoryService.Application.Departments.UseCases.Create;

public record CreateDepartmentCommand(CreateDepartmentRequest Request) : ICommand;