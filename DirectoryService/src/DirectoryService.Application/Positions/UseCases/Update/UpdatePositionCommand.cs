using DirectoryService.Contracts.Positions.Requests;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Positions.UseCases.Update;

public record UpdatePositionCommand(Guid Id, UpdatePositionRequest Request) : ICommand;