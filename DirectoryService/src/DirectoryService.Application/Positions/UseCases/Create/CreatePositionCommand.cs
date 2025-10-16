using DirectoryService.Contracts.Positions.Requests;
using Shared.Abstractions;

namespace DirectoryService.Application.Positions.UseCases.Create;

public record CreatePositionCommand(CreatePositionRequest Request) : ICommand;