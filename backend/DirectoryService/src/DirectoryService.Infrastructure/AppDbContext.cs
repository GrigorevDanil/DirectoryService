using System.Data.Common;
using DirectoryService.Application.Database;
using DirectoryService.Domain.DepartmentLocations;
using DirectoryService.Domain.DepartmentPositions;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Positions;
using Microsoft.EntityFrameworkCore;
using SharedService.Core.Database;

namespace DirectoryService.Infrastructure;

public class AppDbContext(string connectionString) : DbContext, IReadDbContext, IDbConnectionFactory
{
    public DbSet<Department> Departments => Set<Department>();

    public DbSet<Location> Locations => Set<Location>();

    public DbSet<Position> Positions => Set<Position>();

    public DbSet<DepartmentLocation> DepartmentLocations => Set<DepartmentLocation>();

    public DbSet<DepartmentPosition> DepartmentPositions => Set<DepartmentPosition>();

    public IQueryable<Department> DepartmentsRead => Set<Department>().AsNoTracking().AsQueryable();

    public IQueryable<Location> LocationsRead => Set<Location>().AsNoTracking().AsQueryable();

    public IQueryable<Position> PositionsRead => Set<Position>().AsNoTracking().AsQueryable();

    public IQueryable<DepartmentLocation> DepartmentLocationsRead => Set<DepartmentLocation>().AsNoTracking().AsQueryable();

    public IQueryable<DepartmentPosition> DepartmentPositionsRead => Set<DepartmentPosition>().AsNoTracking().AsQueryable();

    public DbConnection GetDbConnection() => Database.GetDbConnection();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("ltree");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}