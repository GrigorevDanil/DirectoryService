using System.ComponentModel;
using DirectoryService.Application.Departments.Queries.GetChildrenDepartments;
using DirectoryService.Application.Departments.Queries.GetRootDepartments;
using DirectoryService.Application.Departments.Queries.GetTopFiveDepartmentsWithMostPositions;
using DirectoryService.Application.Departments.UseCases.Create;
using DirectoryService.Application.Departments.UseCases.Delete;
using DirectoryService.Application.Departments.UseCases.Move;
using DirectoryService.Application.Departments.UseCases.SetLocations;
using DirectoryService.Contracts.Departments.Dtos;
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

    [HttpPatch("{id:guid}/parent")]
    [ProducesResponseType<Envelope<Guid>>(200)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Перенос подразделений")]
    public async Task<EndpointResult<Guid>> Move(
        [FromRoute][Description("Идентификатор подразделения")] Guid id,
        [FromBody] [Description("Данные подразделения")] MoveDepartmentRequest request,
        [FromServices] MoveDepartmentHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new MoveDepartmentCommand(id, request), cancellationToken);

    [HttpGet("top-positions")]
    [ProducesResponseType<Envelope<PaginationEnvelope<DepartmentDto>>>(200)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Получить топ 5 подразделений с наибольшим количеством позиций")]
    public async Task<EndpointResult<DepartmentDto[]>> GetTopFiveDepartmentsWithMostPositions(
        [FromServices] GetTopFiveDepartmentsWithMostPositionsHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(cancellationToken);

    [HttpGet("roots")]
    [ProducesResponseType<Envelope<PaginationEnvelope<DepartmentDto>>>(200)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Получение корневых отделов с предзагрузкой детей")]
    public async Task<EndpointResult<PaginationEnvelope<DepartmentDto>>> GetRoots(
        [FromQuery] GetRootDepartmentsRequest request,
        [FromServices] GetRootDepartmentsHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new GetRootDepartmentsQuery(request), cancellationToken);

    [HttpGet("{parentId:guid}/children")]
    [ProducesResponseType<Envelope<PaginationEnvelope<DepartmentDto>>>(200)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Получить детей по родительскому отделу")]
    public async Task<EndpointResult<PaginationEnvelope<DepartmentDto>>> GetChildren(
        [FromRoute][Description("Идентификатор родительского подразделения")] Guid parentId,
        [FromQuery] GetChildrenDepartmentsRequest request,
        [FromServices] GetChildrenDepartmentsHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new GetChildrenDepartmentsQuery(parentId, request), cancellationToken);

    [HttpDelete("{id:guid}")]
    [ProducesResponseType<Envelope<Guid>>(200)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Удаление подразделения")]
    public async Task<EndpointResult<Guid>> Delete(
        [FromRoute][Description("Идентификатор подразделения")] Guid id,
        [FromServices] DeleteDepartmentHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new DeleteDepartmentCommand(id), cancellationToken);
}