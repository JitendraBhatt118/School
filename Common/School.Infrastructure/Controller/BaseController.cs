using Microsoft.AspNetCore.Mvc;
using School.ViewModel.Common;

namespace School.Infrastructure.Controller;

/// <summary>
/// Represents base controller for all microservice endpoints.
/// Success and Fail reponses will be used at all the end points.
/// </summary>
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Response with success message
    /// </summary>
    /// <typeparam name="T">Generic type</typeparam>
    /// <param name="data">T type</param>
    /// <param name="message">Success message</param>
    /// <returns></returns>
    protected IActionResult Success<T>(T data,
            string message = Universal.ApiGeneralSuccessMessage)
        => Ok(ApiResponse<T>.SuccessResponse(data, message));

    /// <summary>
    /// Response with fail message
    /// </summary>
    /// <param name="errors">Error object</param>
    /// <param name="message">Error message</param>
    /// <param name="statusCode">Status code</param>
    /// <returns></returns>
    protected IActionResult Fail(object errors,
            string message = Universal.ApiGeneralFailMessage,
            int statusCode = 400)
        => StatusCode(statusCode,
            ApiResponse<object>.FailureResponse(errors, message, statusCode));
}
