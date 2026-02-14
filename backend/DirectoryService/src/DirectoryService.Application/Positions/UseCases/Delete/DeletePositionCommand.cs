using SharedService.Core.Handlers;

namespace DirectoryService.Application.Positions.UseCases.Delete;

public record DeletePositionCommand(Guid Id): ICommand;