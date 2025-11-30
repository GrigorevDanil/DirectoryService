using CSharpFunctionalExtensions;
using SharedService.SharedKernel;

namespace DirectoryService.Domain.Departments.ValueObjects;

/// <summary>
/// Путь подразделения.
/// </summary>
public record Path
{
    private const char SEPARATOR = '.';

    private const short START_DEPTH = 0;

    private Path(string value, short depth)
    {
        Value = value;
        Depth = depth;
    }

    public string Value { get; private set; } = string.Empty;

    public short Depth { get; private set; }

    /// <summary>
    /// Создает новый объект <see cref="Path"/> для родителя.
    /// </summary>
    /// <param name="identifier">Входящее короткое название подразделения.</param>
    /// <returns>Новый объект <see cref="Path"/> или ошибку <see cref="Error"/>.</returns>
    public static Path CreateParent(Identifier identifier)
    {
        return new Path(identifier.Value, START_DEPTH);
    }

    /// <summary>
    /// Создает новый объект <see cref="Path"/> для дочернего подразделения.
    /// </summary>
    /// <param name="identifier">Входящее короткое название подразделения.</param>
    /// <returns>Новый объект <see cref="Path"/> или ошибку <see cref="Error"/>.</returns>
    public Path CreateChild(Identifier identifier)
    {
        string path = $"{Value}{SEPARATOR}{identifier.Value}";

        string[] depth = path.Split(SEPARATOR);

        return new Path(path, (short)(depth.Length - 1));
    }

    /// <summary>
    /// Создает новый объект <see cref="Path"/> из строки.
    /// </summary>
    /// <param name="path">Входящий путь.</param>
    /// <returns>Новый объект <see cref="Path"/> или ошибку <see cref="Error"/>.</returns>
    public static Result<Path, Error> Of(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return GeneralErrors.ValueIsRequired("department.path");

        if (path.StartsWith(SEPARATOR) || path.EndsWith(SEPARATOR))
            return GeneralErrors.ValueIsInvalid("Path cannot start or end with separator", "department.path");

        string[] segments = path.Split(SEPARATOR);

        foreach (var segment in segments)
        {
            var identifierResult = Identifier.Of(segment);
            if (identifierResult.IsFailure)
                return identifierResult.Error;
        }

        short depth = (short)(segments.Length - 1);

        return new Path(path, depth);
    }
}