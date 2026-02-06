using CSharpFunctionalExtensions;
using SharedService.SharedKernel;

namespace DirectoryService.Application.SoftDelete;

public interface IDeletedRecordsCleanerService
{
    Task<UnitResult<Error>> Process(CancellationToken cancellationToken = default);
}