using DirectoryService.Application.SoftDelete;
using DirectoryService.Infrastructure.BackgroundServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DirectoryService.Infrastructure.SoftDelete;

public class DeletedRecordsCleanerBackgroundService : BackgroundService
{
    private readonly SoftDeleteSettings _settings;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly ILogger<DeletedRecordsCleanerBackgroundService> _logger;

    public DeletedRecordsCleanerBackgroundService(
        ILogger<DeletedRecordsCleanerBackgroundService> logger,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<SoftDeleteSettings> options)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _settings = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DeletedRecordsCleanerBackgroundService is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            int interval = BackgroundServiceHelper.GetIntervalExecute(
                _settings.Years,
                _settings.Months,
                _settings.Days,
                _settings.Hours,
                _settings.Minutes,
                _settings.Seconds);

            await Task.Delay(interval, stoppingToken);

            await Task.Delay(5000, stoppingToken);

            await using var scope = _serviceScopeFactory.CreateAsyncScope();

            var deletedRecordsCleanerService = scope.ServiceProvider.GetRequiredService<IDeletedRecordsCleanerService>();

            var result = await deletedRecordsCleanerService.Process(stoppingToken);

            if (result.IsSuccess)
                _logger.LogInformation("Deleted records have been deleted.");
        }

        await Task.CompletedTask;
    }
}