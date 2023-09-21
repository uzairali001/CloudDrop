namespace CloudDrop.Shared.Models.Responses;
public class UploadSessionResponse
{
    public required string SessionId { get; set; }
    public required Uri UpdateUrl { get; set; }
    public required DateTime ExpirationDateTime { get; set; }
}
