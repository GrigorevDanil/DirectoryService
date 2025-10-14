using System.Text.Json;
using FluentValidation.Results;

namespace Shared.Validation;

public static class ValidationExtensions
{
    /// <summary>
    /// Преобразует ответ валидации <see cref="ValidationResult"/> в список ошибок <see cref="Errors"/>
    /// </summary>
    /// <param name="validationResult">Ответ валидации.</param>
    /// <returns>Список ошибок <see cref="Errors"/>.</returns>
    public static Errors ToErrors(this ValidationResult validationResult)
    {
        var errors = from validationError in validationResult.Errors
            let error = JsonSerializer.Deserialize<Error>(validationError.ErrorMessage)
            select Error.Validation(error.Message,  error.InvalidField ?? validationError.PropertyName, error.Code);

        return errors.ToList();
    }
}