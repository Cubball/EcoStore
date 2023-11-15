using EcoStore.BLL.Validation.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EcoStore.Presentation.Extensions;

public static class ValidationExceptionExtensions
{
    public static void AddErrorsToModelState(this ValidationException exception, ModelStateDictionary modelState)
    {
        foreach (var error in exception.Errors)
        {
            modelState.AddModelError(error.Property, error.Message);
        }
    }
}