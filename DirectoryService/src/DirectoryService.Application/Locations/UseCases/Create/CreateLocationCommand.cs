using DirectoryService.Contracts.Locations.Requests;
using Shared.Abstractions;

namespace DirectoryService.Application.Locations.UseCases.Create;

public record CreateLocationCommand(CreateLocationRequest Request) : ICommand;