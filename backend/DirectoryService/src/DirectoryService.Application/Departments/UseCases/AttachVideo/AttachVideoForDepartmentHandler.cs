using CSharpFunctionalExtensions;
using DirectoryService.Application.Constants;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Departments.ValueObjects;
using FileService.Contracts.HttpCommunication;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using SharedService.Core.Caching;
using SharedService.Core.Database;
using SharedService.Core.Handlers;
using SharedService.Core.Validation;
using SharedService.SharedKernel;

namespace DirectoryService.Application.Departments.UseCases.AttachVideo;

public class AttachVideoForDepartmentHandler : ICommandHandler<AttachVideoForDepartmentCommand, Guid>
{
    private readonly IDepartmentRepository _departmentRepository;

    private readonly IFileCommunicationService _fileCommunicationService;

    private readonly IValidator<AttachVideoForDepartmentCommand> _validator;

    private readonly ILogger<AttachVideoForDepartmentHandler> _logger;

    private readonly ITransactionManager _transactionManager;

    private readonly ICacheService _cache;

    public AttachVideoForDepartmentHandler(
        IDepartmentRepository departmentRepository,
        IFileCommunicationService fileCommunicationService,
        IValidator<AttachVideoForDepartmentCommand> validator,
        ILogger<AttachVideoForDepartmentHandler> logger,
        ITransactionManager transactionManager,
        ICacheService cache)
    {
        _departmentRepository = departmentRepository;
        _fileCommunicationService = fileCommunicationService;
        _validator = validator;
        _logger = logger;
        _transactionManager = transactionManager;
        _cache = cache;
    }

    public async Task<Result<Guid, Errors>> Handle(
        AttachVideoForDepartmentCommand command,
        CancellationToken cancellationToken = new CancellationToken())
    {
        ValidationResult validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrors();

        Result<bool, Errors> existsResult = await _fileCommunicationService.CheckMediaAssetExists(command.Request.VideoId, cancellationToken);

        if (existsResult.IsFailure)
            return existsResult.Error;

        var departmentId = DepartmentId.Of(command.DepartmentId);

        Result<Department, Error> getDepartmentResult = await _departmentRepository.GetByAsync(x => x.Id == departmentId && x.IsActive == true, cancellationToken);

        if (getDepartmentResult.IsFailure)
            return getDepartmentResult.Error.ToErrors();

        Department department = getDepartmentResult.Value;

        department.AttachVideo(command.Request.VideoId);

        UnitResult<Error> commitedResult = await _transactionManager.SaveChangesAsyncWithResult(cancellationToken);

        if (commitedResult.IsFailure)
            return commitedResult.Error.ToErrors();

        await _cache.RemoveByPrefixAsync(CachingKeys.DEPARTMENTS_KEY, cancellationToken);

        _logger.LogInformation("Video with id {VideoId} was attached to department with id {DepartmentId}", command.Request.VideoId, command.DepartmentId);

        return command.DepartmentId;
    }
}