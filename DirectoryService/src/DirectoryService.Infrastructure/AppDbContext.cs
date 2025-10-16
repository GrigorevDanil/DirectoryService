using CSharpFunctionalExtensions;
using DirectoryService.Domain.DepartmentLocations;
using DirectoryService.Domain.DepartmentPositions;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Positions;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Constants;

namespace DirectoryService.Infrastructure;

public class AppDbContext(string connectionString) : DbContext
{
    public DbSet<Department> Departments => Set<Department>();

    public DbSet<Location> Locations => Set<Location>();

    public DbSet<Position> Positions => Set<Position>();

    public DbSet<DepartmentLocation> DepartmentLocations => Set<DepartmentLocation>();

    public DbSet<DepartmentPosition> DepartmentPositions => Set<DepartmentPosition>();

    /// <summary>
    /// Сохранить изменения в бд с результатом
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения сохранения.</returns>
    public async Task<UnitResult<Error>> SaveChangesAsyncWithResult(CancellationToken cancellationToken = default)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException dbUpdateEx)
        {
            if (dbUpdateEx.InnerException?.Data[InnerExceptionDataConstants.SQL_STATE]!.ToString() == SqlStates.UNIQUE_CONSTRAINT_VIOLATION)
                return GeneralErrors.Conflict();

            return GeneralErrors.Failure(dbUpdateEx.Message);
        }
        catch (Exception ex)
        {
            return GeneralErrors.Failure(ex.Message);
        }

        return Result.Success<Error>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}