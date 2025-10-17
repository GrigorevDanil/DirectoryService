using System.ComponentModel;
using DirectoryService.Application.Departments.UseCases.Create;
using DirectoryService.Application.Departments.UseCases.UpdateLocations;
using DirectoryService.Contracts.Departments.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.EndpointResults;

namespace DirectoryService.Presenters.Controllers;

[ApiController]
[Route("api/departments")]
[Tags("Department - Работа с подразделениями")]
public class DepartmentsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<Envelope<Guid>>(201)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Создать подразделение")]
    public async Task<EndpointResult<Guid>> Create(
        [FromBody][Description("Данные подразделения")] CreateDepartmentRequest request,
        [FromServices] CreateDepartmentHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new CreateDepartmentCommand(request), cancellationToken);

    [HttpPatch("{id:guid}/locations")]
    [ProducesResponseType<Envelope<Guid>>(200)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Установить локации для подразделения")]
    public async Task<EndpointResult<Guid>> SetLocation(
        [FromRoute][Description("Идентификатор подразделения")] Guid id,
        [FromBody][Description("Данные подразделения")] SetLocationsForDepartmentRequest request,
        [FromServices] SetLocationsForDepartmentHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new SetLocationsForDepartmentCommand(id, request), cancellationToken);
}