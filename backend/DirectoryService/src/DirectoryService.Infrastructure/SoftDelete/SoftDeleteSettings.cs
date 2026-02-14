namespace DirectoryService.Infrastructure.SoftDelete;

public class SoftDeleteSettings
{
    public int Years { get; init; }

    public int Months { get; init; }

    public int Days { get; init; } = 1;

    public int Hours { get; init; } = 2;

    public int Minutes { get; init; }

    public int Seconds { get; init; }
}