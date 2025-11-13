using Shared.Abstractions;

namespace DirectoryService.Application.Departments.UseCases.Delete;

public record DeleteDepartmentCommand(Guid Id) : ICommand;