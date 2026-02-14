using DirectoryService.Contracts.Positions.Requests;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Positions.UseCases.RemoveDepartments;

public record RemoveDepartmentsFromPositionCommand(Guid Id, RemoveDepartmentFromPositionRequest Request) : ICommand;