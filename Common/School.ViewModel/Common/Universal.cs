namespace School.ViewModel.Common;

/// <summary>
/// Represents constants which will be used in all the
/// micro-services. Please do not add any other constants
/// which are not related to micro-service infrastructure or
/// common architecture of micro-services.
/// </summary>
public static class Universal
{
    public const string ApiGeneralSuccessMessage = "Request successful.";
    public const string ApiGeneralFailMessage = "Unable to process your request.";
    public const string DapperTransactionErrorMessage = "Transaction connection is null.";
}
