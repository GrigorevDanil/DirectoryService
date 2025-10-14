using System.Text.Json;
using CSharpFunctionalExtensions;
using FluentValidation;

namespace Shared.Validation;

public static class CustomValidators
{
    /// <summary>
    /// Метод расширения позволяющий проверить входящий параметр на соответствие указанному методу создания ValueObject
    /// </summary>
    /// <param name="ruleBuilder">Строитель правил.</param>
    /// <param name="factoryMethod">Фабричный метод создания ValueObject.</param>
    /// <typeparam name="T">T.</typeparam>
    /// <typeparam name="TElement">TElement.</typeparam>
    /// <typeparam name="TValueObject">TValueObject.</typeparam>
    /// <returns>Добавляет новую ошибку проверки для указанного входящего параметра.</returns>
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder, Func<TElement, Result<TValueObject, Error>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            Result<TValueObject, Error> result = factoryMethod(value);

            if (result.IsSuccess)
                return;

            context.AddFailure(JsonSerializer.Serialize(result.Error));
        });
    }

    /// <summary>
    /// Метод расширения позволяющий проверить входящий параметр на соответствие указанному методу создания ValueObject
    /// </summary>
    /// <param name="ruleBuilder">Строитель правил.</param>
    /// <param name="factoryMethod">Фабричный метод создания ValueObject.</param>
    /// <typeparam name="T">T.</typeparam>
    /// <typeparam name="TElement">TElement.</typeparam>
    /// <typeparam name="TValueObject">TValueObject.</typeparam>
    /// <returns>Добавляет новую ошибку проверки для указанного входящего параметра.</returns>
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder, Func<TElement, Result<TValueObject, Errors>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            Result<TValueObject, Errors> result = factoryMethod(value);

            if (result.IsSuccess)
                return;

            foreach (Error error in result.Error)
                context.AddFailure(JsonSerializer.Serialize(error));
        });
    }

    /// <summary>
    /// Метод расширения позволяющий указать ошибку <see cref="Error"/> которая вызовется исходя из условий валидации
    /// </summary>
    /// <param name="rule">Текущее правило.</param>
    /// <param name="error">Ошибка.</param>
    /// <typeparam name="T">T.</typeparam>
    /// <typeparam name="TProperty">TProperty.</typeparam>
    /// <returns>Указывает ошибку, которая будет вызвана в случае сбоя проверки. Применяется только к правилам.</returns>
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, Error error)
    {
        return rule.WithMessage(JsonSerializer.Serialize(error));
    }
}