using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using TorneioLeft4Dead2.Shared.Helpers;

namespace TorneioLeft4Dead2FunctionApp.Exceptions;

public class FriendlyResult
{
    private FriendlyResult(ValidationException validationException)
    {
        Message = validationException.Message;
        Errors = new Dictionary<string, List<string>>();

        foreach (var error in validationException.Errors)
        {
            var propertyName = error.PropertyName;
            propertyName = FirstLetterLowerCase(propertyName);

            if (string.IsNullOrEmpty(propertyName))
                propertyName = "_entity";

            if (!Errors.ContainsKey(propertyName))
                Errors.Add(propertyName, new List<string>());

            Errors[propertyName].Add(error.ErrorMessage);
        }
    }

    private FriendlyResult(Exception exception)
    {
        Message = exception.Message;
        Errors = new Dictionary<string, List<string>>();

        const string propertyName = "exception";

        Errors.Add(propertyName, new List<string>
        {
            exception.Message
        });
    }

    public string Message { get; }
    public Dictionary<string, List<string>> Errors { get; }
    public List<string> AllErros => Errors?.SelectMany(erro => erro.Value).Distinct().ToList();

    private static string FirstLetterLowerCase(string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName))
            return propertyName;

        var values = propertyName.Split(".").Select(property => property.FirstLetterLowerCase());

        return string.Join(".", values);
    }

    public static FriendlyResult New(Exception exception)
    {
        if (exception is ValidationException validationException)
            return new FriendlyResult(validationException);

        return new FriendlyResult(exception);
    }
}