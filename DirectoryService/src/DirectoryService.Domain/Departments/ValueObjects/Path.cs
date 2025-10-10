using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.Departments.ValueObjects;

/// <summary>
/// Путь подразделения
/// </summary>
public record Path
{
    private const char SEPARATOR = '.';

    private const short START_DEPTH = 1;

    private Path(string value, short depth)
    {
        Value = value;
        Depth = depth;
    }

    public string Value { get; private set; } = string.Empty;

    public short Depth { get; private set; }

    /// <summary>
    /// Создает новый объект "Путь подразделения" для родителя
    /// </summary>
    /// <param name="value">Входящее значение.</param>
    /// <returns>Новый объект или ошибка.</returns>
    public static Result<Path> Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Path>("Value cannot be empty");

        string[] segments = value.Split(SEPARATOR);

        if (segments.Any(string.IsNullOrWhiteSpace))
            return Result.Failure<Path>("Path segments cannot be empty");

        short depth = (short)segments.Length;
        string normalizedValue = string.Join(SEPARATOR, segments);

        return new Path(normalizedValue, depth);
    }

    /// <summary>
    /// Переход к вложенному
    /// </summary>
    /// <param name="child">Дочернее значение.</param>
    /// <returns>Новый объект или ошибка.</returns>
    public Result<Path> Descending(string child)
    {
        if (child.Contains(SEPARATOR))
            return Result.Failure<Path>("Child cannot contain the separator");

        string newValue = $"{Value}{SEPARATOR}{child}";
        short newDepth = (short)(Depth + 1);

        return new Path(newValue, newDepth);
    }

    /// <summary>
    /// Переход к родителю
    /// </summary>
    /// <returns>Новый объект или ошибка.</returns>
    public Result<Path> Ascending()
    {
        if (Depth <= START_DEPTH)
            return Result.Failure<Path>("Path is the root");

        string[] segments = Value.Split(SEPARATOR);
        var parentSegments = segments.Take(segments.Length - 1);
        string newValue = string.Join(SEPARATOR, parentSegments);
        short newDepth = (short)(Depth - 1);

        return new Path(newValue, newDepth);
    }

    /// <summary>
    /// Изменить название сегмента в пути
    /// </summary>
    /// <param name="oldSegment">Старое название сегмента.</param>
    /// <param name="newSegment">Новое название сегмента.</param>
    /// <returns>Новый объект или ошибка.</returns>
    public Result<Path> ChangeSegment(string oldSegment,  string newSegment)
    {
        if (string.IsNullOrWhiteSpace(oldSegment) || string.IsNullOrWhiteSpace(newSegment) || newSegment.Contains(SEPARATOR))
            return Result.Failure<Path>("Segments are incorrect");

        string[] segments = Value.Split(SEPARATOR);
        int index = Array.IndexOf(segments, oldSegment);

        if (index == -1)
            return Result.Failure<Path>("Old segment not found in the path");

        segments[index] = newSegment;
        string newValue = string.Join(SEPARATOR, segments);

        return new Path(newValue, Depth);
    }

};