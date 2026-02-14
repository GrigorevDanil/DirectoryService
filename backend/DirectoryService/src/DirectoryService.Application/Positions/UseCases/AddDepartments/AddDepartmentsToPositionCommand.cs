using DirectoryService.Contracts.Positions.Requests;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Positions.UseCases.AddDepartments;

public record AddDepartmentsToPositionCommand(Guid Id, AddDepartmentToPositionRequest Request) : ICommand;