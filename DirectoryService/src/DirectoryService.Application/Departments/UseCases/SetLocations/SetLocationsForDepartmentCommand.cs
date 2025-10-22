using DirectoryService.Contracts.Departments.Requests;
using Shared.Abstractions;

namespace DirectoryService.Application.Departments.UseCases.SetLocations;

public record SetLocationsForDepartmentCommand(Guid DepartmentId, SetLocationsForDepartmentRequest ForDepartmentRequest) : ICommand;