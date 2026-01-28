using System.ComponentModel;
using DirectoryService.Application.Positions.Queries.Get;
using DirectoryService.Application.Positions.Queries.GetDetail;
using DirectoryService.Application.Positions.UseCases.AddDepartment;
using DirectoryService.Application.Positions.UseCases.Create;
using DirectoryService.Application.Positions.UseCases.Delete;
using DirectoryService.Application.Positions.UseCases.RemoveDepartment;
using DirectoryService.Application.Positions.UseCases.Update;
using DirectoryService.Contracts.Positions.Dtos;
using DirectoryService.Contracts.Positions.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedService.Framework.Endpoints;
using SharedService.SharedKernel;

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

    [HttpGet("{id:guid}")]
    [ProducesResponseType<PaginationEnvelope<PositionDto>>(200)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Получить позицию по идентификатору")]
    public async Task<EndpointResult<PositionDetailDto?>> GetDetail(
        [FromRoute] Guid id,
        [FromServices] GetDetailPositionHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new GetDetailPositionQuery(id), cancellationToken);

    [HttpGet]
    [ProducesResponseType<PaginationEnvelope<PositionDto>>(200)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Получить позиции")]
    public async Task<EndpointResult<PaginationEnvelope<PositionDto>>> Get(
        [FromQuery] GetPositionsRequest request,
        [FromServices] GetPositionsHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new GetPositionsQuery(request), cancellationToken);

    [HttpPut("{id:guid}")]
    [ProducesResponseType<Envelope<Guid>>(200)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Обновить информацию о позиции")]
    public async Task<EndpointResult<Guid>> Update(
        [FromRoute][Description("Идентификатор позиции")] Guid id,
        [FromBody][Description("Данные позиции")] UpdatePositionRequest request,
        [FromServices] UpdatePositionHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new UpdatePositionCommand(id, request), cancellationToken);

    [HttpDelete("{id:guid}")]
    [ProducesResponseType<Envelope<Guid>>(200)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Удаление локации")]
    public async Task<EndpointResult<Guid>> Delete(
        [FromRoute][Description("Идентификатор позиции")] Guid id,
        [FromServices] DeletePositionHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new DeletePositionCommand(id), cancellationToken);

    [HttpPost("{id:guid}/department")]
    [ProducesResponseType<Envelope<Guid>>(201)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Добавить подразделения к позиции")]
    public async Task<EndpointResult<Guid>> AddDepartments(
        [FromRoute][Description("Идентификатор позиции")] Guid id,
        AddDepartmentToPositionRequest request,
        [FromServices] AddDepartmentsToPositionHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new AddDepartmentsToPositionCommand(id, request), cancellationToken);

    [HttpDelete("{id:guid}/department")]
    [ProducesResponseType<Envelope<Guid>>(200)]
    [ProducesResponseType<Envelope>(400)]
    [ProducesResponseType<Envelope>(500)]
    [ProducesResponseType<Envelope>(409)]
    [EndpointSummary("Удалить подразделения из позиции")]
    public async Task<EndpointResult<Guid>> RemoveDepartments(
        [FromRoute][Description("Идентификатор позиции")] Guid id,
        RemoveDepartmentFromPositionRequest request,
        [FromServices] RemoveDepartmentsFromPositionHandler handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new RemoveDepartmentsFromPositionCommand(id, request), cancellationToken);
}