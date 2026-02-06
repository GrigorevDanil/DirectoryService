using DirectoryService.Contracts.Positions.Requests;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Positions.UseCases.RemoveDepartment;

public record RemoveDepartmentsFromPositionCommand(Guid Id, RemoveDepartmentFromPositionRequest Request) : ICommand;