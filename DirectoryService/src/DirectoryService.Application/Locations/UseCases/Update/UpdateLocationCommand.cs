using DirectoryService.Contracts.Locations.Requests;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Locations.UseCases.Update;

public record UpdateLocationCommand(Guid LocationId, UpdateLocationRequest Request) : ICommand;