using Microsoft.AspNetCore.Http;

namespace Shared.EndpointResults;

/// <summary>
/// Результат с ошибкой
/// </summary>
public sealed class ErrorsResult : IResult
{
    private readonly Errors _errors;

    public ErrorsResult(Error error)
    {
        _errors = error.ToErrors();
    }

    public ErrorsResult(Errors errors)
    {
        _errors = errors;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        if (!_errors.Any())
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return httpContext.Response.WriteAsJsonAsync(Envelope.Error(_errors));
        }

        var distinctErrors = _errors
            .Select(x => x.Type)
            .Distinct()
            .ToList();

        int statusCode = distinctErrors.Count > 1 ?
            StatusCodes.Status500InternalServerError : GetStatusCodeForErrorType(distinctErrors.First());

        var envelope = Envelope.Error(_errors);
        httpContext.Response.StatusCode = statusCode;

        return httpContext.Response.WriteAsJsonAsync(envelope);
    }

    /// <summary>
    /// Получает статус код по типу ошибки
    /// </summary>
    /// <param name="errorType">Тип ошибки.</param>
    /// <returns>Статус код.</returns>
    private static int GetStatusCodeForErrorType(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.VALIDATION => StatusCodes.Status400BadRequest,
            ErrorType.NOT_FOUND => StatusCodes.Status404NotFound,
            ErrorType.CONFLICT => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
}

