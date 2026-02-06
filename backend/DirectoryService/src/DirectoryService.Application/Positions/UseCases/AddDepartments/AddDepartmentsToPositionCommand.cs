using DirectoryService.Contracts.Positions.Requests;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Positions.UseCases.AddDepartment;

public record AddDepartmentsToPositionCommand(Guid Id, AddDepartmentToPositionRequest Request) : ICommand;