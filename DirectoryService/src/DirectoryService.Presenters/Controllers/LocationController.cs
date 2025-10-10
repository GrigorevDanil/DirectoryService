using DirectoryService.Application.Abstractions.Locations;
using DirectoryService.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presenters.Controllers;

[ApiController]
[Route("api/location")]
public class LocationController : ControllerBase
{
    private readonly ILocationsService _locationsService;

    public LocationController(ILocationsService locationsService) => _locationsService = locationsService;

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] LocationDto locationDto, CancellationToken cancellationToken)
    {
        var result = await _locationsService.AddAsync(locationDto, cancellationToken);

        if (result.IsFailure) 
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}