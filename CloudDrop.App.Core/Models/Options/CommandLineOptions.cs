namespace CloudDrop.App.Core.Models.Options;
public class CommandLineOptions
{
    public string? ApiUrl { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? FilesDirectory { get; set; }
    public bool RunImmediately { get; set; }
}
