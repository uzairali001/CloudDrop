namespace CloudDrop.Api.Core.Models.Responses;
public class UploadSessionResponse
{
    public required string SessionId { get; set; }
    public required Uri UpdateUrl { get; set; }
    public DateTime ExpirationDateTime { get; set; }
}
