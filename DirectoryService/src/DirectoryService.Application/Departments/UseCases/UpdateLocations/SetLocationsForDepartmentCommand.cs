using DirectoryService.Contracts.Departments.Requests;
using Shared.Abstractions;

namespace DirectoryService.Application.Departments.UseCases.UpdateLocations;

public record SetLocationsForDepartmentCommand(Guid DepartmentId, SetLocationsForDepartmentRequest ForDepartmentRequest) : ICommand;