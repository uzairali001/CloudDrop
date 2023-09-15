using CloudDrop.App.Core.Constants;
using CloudDrop.App.Core.Entities;
using CloudDrop.App.Core.Models.Options;
using CloudDrop.App.Core.Services.General;
using CloudDrop.App.Installer.Constants;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

using System.Diagnostics;

namespace CloudDrop.App.Installer;
public class Install
{
    private readonly string _appFolder = AppConstants.AppDirectory;
    private readonly AppAuthenticationService _appAuthenticationService;
    private readonly IServiceScopeFactory _scopeFactory;
    private CommandLineOptions _options;

    public Install(AppAuthenticationService appAuthenticationService,
       IServiceScopeFactory scopeFactory)
    {
        _appAuthenticationService = appAuthenticationService;
        _scopeFactory = scopeFactory;
    }

    public async Task<int> OnExecuteAsync(CommandLineOptions options)
    {
        try
        {
            _options = options;
            string appPath = Path.ChangeExtension(Path.Combine(_appFolder, AppConstants.AppName), "exe");

            if (File.Exists(appPath))
            {
                Console.WriteLine("Please uninstall the existing installation of the application before proceeding with the new installation.");
                return 0;
            }

            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<CloudDropDbContext>();
            context.Database.EnsureCreated();


            Console.WriteLine("Configuring...");

            // Set BaseUrl
            await ConfigureBaseUrlAsync();

            // Set username/email and password
            // Authenticate User
            bool isSet = await ConfigureUserAsync();
            if (isSet is false)
            {
                return 1;
            }

            Console.WriteLine();
            Console.WriteLine("Installing the app...");

            Console.WriteLine("Copying files...");
            CopyApplicationFiles();
            Console.WriteLine("Files copied to " + _appFolder);

            Console.WriteLine();

            Console.WriteLine("Setting up as startup app...");
            RegisterAsStartupApp();
            Console.WriteLine("Done.");

            Console.WriteLine();

            bool confirmation = _options.RunImmediately || AskToRunTheApp();

            if (confirmation is false)
            {
                Console.WriteLine("The app will run after a restart.");
                return 0;
            }

            Process? process = LaunchTheApp();
            if (process is null)
            {
                Console.WriteLine("Unable to start the app");
                return 1;
            }

            Console.WriteLine($"The app has been started with PID: {process.Id}.");
            return 0;
        }
        catch (Exception e)
        {
            Console.WriteLine();
            Console.WriteLine(e);
            return 1;
        }
    }

    private async Task<bool> ConfigureUserAsync()
    {
        string? username = _options.Username ?? AskForUsernameOrEmail();
        string? password = _options.Password ?? AskForPassword();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Invalid username or password");
            return false;
        }
        Console.WriteLine("Authenticating user...");
        var user = await _appAuthenticationService.AuthenticateUserAsync(username, password);
        if (user is null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.AuthToken))
        {
            Console.WriteLine("Invalid username or password");
            return false;
        }

        _ = _appAuthenticationService.SetAuthenticationTokenAsync(user.AuthToken);
        return true;
    }


    private async Task ConfigureBaseUrlAsync()
    {
        string? baseUrl = _options.BaseUrl ?? AskForBaseURL();
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new Exception("Invalid BaseUrl");
        }

        await _appAuthenticationService.SetBaseUrlAsync(baseUrl);
    }

    private void CopyApplicationFiles()
    {
        Directory.CreateDirectory(_appFolder);

        foreach (string file in Directory.GetFiles("Resources"))
        {
            File.Copy(file, Path.Combine(_appFolder, Path.GetFileName(file)));
        }
    }

    private void RegisterAsStartupApp()
    {
        if (OperatingSystem.IsWindows() is false)
        {
            return;
        }

        string appPath = Path.Combine(_appFolder, AppConstants.AppName + ".exe");

        RegistryKey? rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        rkApp?.SetValue(AppConstants.NameOnRegistry, appPath);
    }

    private string? AskForBaseURL()
    {
        return Prompt.GetString("What is the baseURL? :",
            promptColor: ConsoleColor.White,
            promptBgColor: ConsoleColor.Black);
    }

    private string? AskForUsernameOrEmail()
    {
        return Prompt.GetString("Please input the username or email:",
            promptColor: ConsoleColor.White,
            promptBgColor: ConsoleColor.Black);
    }
    private string? AskForPassword()
    {
        return Prompt.GetPassword("Please input the password:",
            promptColor: ConsoleColor.White,
            promptBgColor: ConsoleColor.Black);
    }


    private string? AskForFilesDirectory()
    {
        return Prompt.GetString("Please input the directory of files to upload",
            promptColor: ConsoleColor.White,
            promptBgColor: ConsoleColor.Black);
    }

    private bool AskToRunTheApp()
    {
        return Prompt.GetYesNo("Do you want to start the app now?",
                defaultAnswer: true,
                promptColor: ConsoleColor.Black,
                promptBgColor: ConsoleColor.White);
    }

    private Process? LaunchTheApp()
    {
        string appPath = Path.Combine(_appFolder, AppConstants.AppName + ".exe");
        return Process.Start(appPath);

        //// Prepare the process to run
        //ProcessStartInfo process = new()
        //{
        //    // Enter in the command line arguments, everything you would enter after the executable name itself
        //    //start.Arguments = arguments;
        //    // Enter the executable to run, including the complete path
        //    FileName = appPath,
        //    // Do you want to show a Console window?
        //    WindowStyle = ProcessWindowStyle.Hidden,
        //    CreateNoWindow = true
        //};

        //return Process.Start(process);
    }
}