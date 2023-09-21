using Microsoft.AspNetCore.Http;

namespace CloudDrop.Shared.Models.Requests;
public class UpdateUploadSessionRequest
{
    public required string SessionId { get; set; }
    public long Size { get; set; }
    public long BytesFrom { get; set; }
    public long BytesTo { get; set; }
    public required IFormFile File { get; set; }
}
