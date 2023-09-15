namespace CloudDrop.App.Core.Constants;

public static class AppConstants
{
    public readonly static string AppDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), BaseFolder);

    public const string BaseFolder = "CloudDrop";
    //public const string MediaDirectory = "Media";

    //public const string MicAudioFilename = "MicAudio.wav";
    //public const string SystemAudioFilename = "SystemAudio.wav";

    public const string NameOnRegistry = "CloudDrop";
    public const string AppName = "CloudDrop";
}
