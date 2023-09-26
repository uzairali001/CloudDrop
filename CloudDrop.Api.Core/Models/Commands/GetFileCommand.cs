namespace CloudDrop.Api.Core.Models.Commands;
public class GetFileCommand
{
    public uint UserId { get; set; }
    public uint FileId { get; set; }
}
