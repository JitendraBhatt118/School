namespace School.ViewModel.Common;

/// <summary>
/// Represents api response model
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }
    public object? Errors { get; init; }
    public int StatusCode { get; init; }

    public static ApiResponse<T> SuccessResponse(T data,
        string message = Universal.ApiGeneralSuccessMessage,
        int statusCode = 200
    ) => new()
    {
        Success = true,
        Message = message,
        Data = data,
        StatusCode = statusCode
    };

    public static ApiResponse<T> FailureResponse(T errors,
        string message = Universal.ApiGeneralFailMessage,
        int statusCode = 400
    ) => new()
    {
        Success = false,
        Message = message,
        Errors = errors,
        StatusCode = statusCode
    };
}
