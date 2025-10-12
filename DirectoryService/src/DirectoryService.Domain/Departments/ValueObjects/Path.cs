using CSharpFunctionalExtensions;
using Shared;

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
    /// Создает новый объект <see cref="Path"/>
    /// </summary>
    /// <param name="value">Входящее значение.</param>
    /// <returns>Новый объект <see cref="Path"/> или ошибку <see cref="Error"/>.</returns>
    public static Result<Path, Error> Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return GeneralErrors.ValueIsEmptyOrInvalidLength("path");

        string[] segments = value.Split(SEPARATOR);

        if (segments.Any(string.IsNullOrWhiteSpace))
            return GeneralErrors.ValueIsEmptyOrInvalidLength("path");

        short depth = (short)segments.Length;
        string normalizedValue = string.Join(SEPARATOR, segments);

        return new Path(normalizedValue, depth);
    }

    /// <summary>
    /// Переход к вложенному
    /// </summary>
    /// <param name="child">Дочернее значение.</param>
    /// <returns>Новый объект <see cref="Path"/> или ошибка <see cref="Error"/>.</returns>
    public Result<Path, Error> Descending(string child)
    {
        if (child.Contains(SEPARATOR))
            return GeneralErrors.ValueIsInvalid("Child cannot contain the separator", "path");

        string newValue = $"{Value}{SEPARATOR}{child}";
        short newDepth = (short)(Depth + 1);

        return new Path(newValue, newDepth);
    }

    /// <summary>
    /// Переход к родителю
    /// </summary>
    /// <returns>Новый объект <see cref="Path"/> или ошибка <see cref="Error"/>.</returns>
    public Result<Path, Error> Ascending()
    {
        if (Depth <= START_DEPTH)
            return GeneralErrors.ValueIsInvalid("Path is the root", "path");

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
    /// <returns>Новый объект <see cref="Path"/> или ошибку <see cref="Error"/>.</returns>
    public Result<Path, Error> ChangeSegment(string oldSegment,  string newSegment)
    {
        if (string.IsNullOrWhiteSpace(oldSegment) || string.IsNullOrWhiteSpace(newSegment) || newSegment.Contains(SEPARATOR))
            return GeneralErrors.ValueIsInvalid("Segments are incorrect", "path");

        string[] segments = Value.Split(SEPARATOR);
        int index = Array.IndexOf(segments, oldSegment);

        if (index == -1)
            return GeneralErrors.ValueIsInvalid("Old segment not found in the path", "path");

        segments[index] = newSegment;
        string newValue = string.Join(SEPARATOR, segments);

        return new Path(newValue, Depth);
    }

};