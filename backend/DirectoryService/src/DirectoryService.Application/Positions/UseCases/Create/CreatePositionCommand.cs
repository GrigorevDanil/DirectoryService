using DirectoryService.Contracts.Positions.Requests;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Positions.UseCases.Create;

public record CreatePositionCommand(CreatePositionRequest Request) : ICommand;