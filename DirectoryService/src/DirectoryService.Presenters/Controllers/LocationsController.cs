using System.ComponentModel;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Locations.UseCases.Create;
using DirectoryService.Contracts.Dtos;
using DirectoryService.Contracts.Locations.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.EndpointResults;

namespace DirectoryService.Presenters.Controllers;

[ApiController]
[Route("api/location")]
[Tags("Location - Работа с локациями")]
public class LocationsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<Envelope<Guid>>(201)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Создать локацию")]
    public async Task<EndpointResult<Guid>> Create(
        [FromBody][Description("Данные локации")] CreateLocationRequest request,
        [FromServices] CreateLocationHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new CreateLocationCommand(request), cancellationToken);
}