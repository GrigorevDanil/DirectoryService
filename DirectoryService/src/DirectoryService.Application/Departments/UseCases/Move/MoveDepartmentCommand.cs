using DirectoryService.Contracts.Departments.Requests;
using Shared.Abstractions;

namespace DirectoryService.Application.Departments.UseCases.Move;

public record MoveDepartmentCommand(Guid DepartmentId, MoveDepartmentRequest Request) : ICommand;