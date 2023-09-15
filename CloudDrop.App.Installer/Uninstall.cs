using CloudDrop.App.Core.Constants;
using CloudDrop.App.Installer.Constants;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Win32;

using System.Diagnostics;

namespace CloudDrop.App.Installer;
public class Uninstall
{
    private readonly string _appFolder = AppConstants.AppDirectory;

    public async Task<int> OnExecuteAsync(Dictionary<string, CommandOption>? options = default)
    {
        try
        {
            CommandOption? cmd = null;
            options?.TryGetValue(CliConstants.NoPrompt, out cmd);

            var prompt = cmd is not null && cmd.HasValue() || Prompt.GetYesNo("Do you want to uninstall our program?\n\nWarning: All saved information will be removed.",
                defaultAnswer: false,
                promptColor: ConsoleColor.Black,
                promptBgColor: ConsoleColor.White);

            if (!prompt)
            {
                return 0;
            }

            Console.WriteLine("Stopping process...");
            StopTheProcess();

            Console.WriteLine("Removing as startup app...");
            RemoveAsStartupApp();

            Console.WriteLine("Removing files...");
            await DeleteAllAppFiles();

            Console.WriteLine("Uninstallation completed.");

            return 0;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }


        return 1;
    }


    private void StopTheProcess()
    {
        string appName = AppConstants.AppName;
        var process = GetProcess(appName);
        if (process is null)
        {
            Console.WriteLine($"{appName} is not running.");
            return;
        }

        process.Kill();
    }

    private Process? GetProcess(string processName)
    {
        return Process.GetProcesses()
            .FirstOrDefault(p => p.ProcessName.Equals(processName));
    }

    private async Task<bool> DeleteAllAppFiles()
    {
        return await TryDeleteDirectory(_appFolder);
    }

    public static async Task<bool> TryDeleteDirectory(string directoryPath, int maxRetries = 10, int millisecondsDelay = 100)
    {
        if (directoryPath == null)
            throw new ArgumentNullException(directoryPath);
        if (maxRetries < 1)
            throw new ArgumentOutOfRangeException(nameof(maxRetries));
        if (millisecondsDelay < 1)
            throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

        for (int i = 0; i < maxRetries; ++i)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    Directory.Delete(directoryPath, true);
                }

                return true;
            }
            catch (IOException)
            {
                await Task.Delay(millisecondsDelay);
            }
            catch (UnauthorizedAccessException)
            {
                await Task.Delay(millisecondsDelay);
            }
        }

        return false;
    }

    private void RemoveAsStartupApp()
    {
        if (OperatingSystem.IsWindows() is false)
        {
            return;
        }

        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true)!;
        registryKey.DeleteValue(AppConstants.NameOnRegistry, false);
    }
}
