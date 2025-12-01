using DirectoryService.Contracts.Departments.Requests;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Departments.UseCases.SetLocations;

public record SetLocationsForDepartmentCommand(Guid DepartmentId, SetLocationsForDepartmentRequest ForDepartmentRequest) : ICommand;