namespace CloudDrop.Api.Core.Models.Responses;
public class FileResponse
{
    public uint Id { get; set; }
    public required string Name { get; set; }
    public required string MimeType { get; set; }
    public long Size { get; set; }
    public DateTime CreatedAt { get; set; }
}
