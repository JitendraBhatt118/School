namespace School.ViewModel.Common;

/// <summary>
/// Represents data process response model
/// </summary>
public class DataProcessResponseModel
{
    #region Properties
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }
    public string? MessageKey { get; set; }
    public long RowId { get; set; }
    public string? RowIdEncrypted { get; set; }
    public List<long>? Ids { get; set; }
    #endregion
}
