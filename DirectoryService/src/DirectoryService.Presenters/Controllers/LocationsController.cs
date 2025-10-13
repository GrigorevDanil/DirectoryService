using System.ComponentModel;
using DirectoryService.Application.Locations;
using DirectoryService.Contracts.Dtos;
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
    private readonly ILocationsService _locationsService;

    public LocationsController(ILocationsService locationsService) => _locationsService = locationsService;

    [HttpPost]
    [ProducesResponseType<Envelope<Guid>>(201)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [EndpointSummary("Создать локацию")]
    public async Task<EndpointResult<Guid>> Create(
        [FromBody][Description("Данные локации")] LocationDto locationDto,
        CancellationToken cancellationToken) =>
        await _locationsService.AddAsync(locationDto, cancellationToken);
}