using DirectoryService.Contracts.Locations.Requests;
using SharedService.Core.Handlers;

namespace DirectoryService.Application.Locations.UseCases.Create;

public record CreateLocationCommand(CreateLocationRequest Request) : ICommand;