namespace DirectoryService.Infrastructure.SoftDelete;

public class SoftDeleteSettings
{
    public int Years { get; init; } = 0;

    public int Months { get; init; } = 0;

    public int Days { get; init; } = 1;

    public int Hours { get; init; } = 2;

    public int Minutes { get; init; } = 0;

    public int Seconds { get; init; } = 0;
}