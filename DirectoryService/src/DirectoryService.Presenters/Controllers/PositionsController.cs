using System.ComponentModel;
using DirectoryService.Application.Positions.UseCases.Create;
using DirectoryService.Contracts.Positions.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.EndpointResults;

namespace DirectoryService.Presenters.Controllers;

[ApiController]
[Route("api/positions")]
[Tags("Position - Работа с позициями(должностями сотрудников)")]
public class PositionsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<Envelope<Guid>>(201)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Создать позицию")]
    public async Task<EndpointResult<Guid>> Create(
        [FromBody][Description("Данные позиции")] CreatePositionRequest request,
        [FromServices] CreatePositionHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new CreatePositionCommand(request), cancellationToken);
}