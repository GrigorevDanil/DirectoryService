using SharedService.Core.Handlers;

namespace DirectoryService.Application.Locations.UseCases.Delete;

public record DeleteLocationCommand(Guid LocationId) : ICommand;