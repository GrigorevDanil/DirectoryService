using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Domain.Departments.ValueObjects;

/// <summary>
/// Путь подразделения
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
    /// Создает новый объект <see cref="Path"/> для родителя
    /// </summary>
    /// <param name="identifier">Входящее короткое название подразделения.</param>
    /// <returns>Новый объект <see cref="Path"/> или ошибку <see cref="Error"/>.</returns>
    public static Path CreateParent(Identifier identifier)
    {
        return new Path(identifier.Value, START_DEPTH);
    }

    /// <summary>
    /// Создает новый объект <see cref="Path"/> для дочернего подразделения
    /// </summary>
    /// <param name="identifier">Входящее короткое название подразделения.</param>
    /// <returns>Новый объект <see cref="Path"/> или ошибку <see cref="Error"/>.</returns>
    public Path CreateChild(Identifier identifier)
    {
        string path = $"{Value}{SEPARATOR}{identifier.Value}";

        string[] depth = path.Split(SEPARATOR);

        return new Path(path, (short)(depth.Length - 1));
    }
};