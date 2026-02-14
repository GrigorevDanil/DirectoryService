using System.ComponentModel;
using Asp.Versioning;
using DirectoryService.Application.Locations.Queries.Get;
using DirectoryService.Application.Locations.UseCases.Create;
using DirectoryService.Application.Locations.UseCases.Delete;
using DirectoryService.Application.Locations.UseCases.Update;
using DirectoryService.Contracts.Locations.Dtos;
using DirectoryService.Contracts.Locations.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedService.Framework.Endpoints;
using SharedService.SharedKernel;

namespace DirectoryService.Presenters.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/locations")]
[ApiVersion("1")]
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

    [HttpGet]
    [ProducesResponseType<Envelope<LocationDto>>(200)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Получить локации")]
    public async Task<EndpointResult<PaginationEnvelope<LocationDto>>> Get(
        [FromQuery] GetLocationsRequest request,
        [FromServices] GetLocationsHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new GetLocationsQuery(request), cancellationToken);

    [HttpPut("{id:guid}")]
    [ProducesResponseType<Envelope<Guid>>(200)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Обновить информацию о локации")]
    public async Task<EndpointResult<Guid>> Update(
        [FromRoute][Description("Идентификатор локации")] Guid id,
        [FromBody][Description("Данные локации")] UpdateLocationRequest request,
        [FromServices] UpdateLocationHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new UpdateLocationCommand(id, request), cancellationToken);

    [HttpDelete("{id:guid}")]
    [ProducesResponseType<Envelope<Guid>>(200)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Удаление локации")]
    public async Task<EndpointResult<Guid>> Delete(
        [FromRoute][Description("Идентификатор локации")] Guid id,
        [FromServices] DeleteLocationHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new DeleteLocationCommand(id), cancellationToken);
}