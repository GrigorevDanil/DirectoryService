using CSharpFunctionalExtensions;
using DirectoryService.Application.Positions;
using DirectoryService.Domain.Positions;
using Shared;

namespace DirectoryService.Infrastructure.Repositories;

public class PositionsRepository : IPositionsRepository
{
    private readonly AppDbContext _dbContext;

    public PositionsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid, Error>> AddPositionAsync(Position position, CancellationToken cancellationToken = default)
    {
        await _dbContext.Positions.AddAsync(position, cancellationToken);

        var saveChangesResult = await _dbContext.SaveChangesAsyncWithResult(cancellationToken);

        if (saveChangesResult.IsFailure)
            return saveChangesResult.Error;

        return position.Id.Value;
    }
}