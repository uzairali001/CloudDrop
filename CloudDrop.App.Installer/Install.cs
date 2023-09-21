using CloudDrop.App.Core.Constants;
using CloudDrop.App.Core.Contracts.Services.Data;
using CloudDrop.App.Core.Entities;
using CloudDrop.App.Core.Models.Dtos;
using CloudDrop.App.Core.Models.Options;
using CloudDrop.App.Core.Services.General;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Win32;

using System.Diagnostics;

namespace CloudDrop.App.Installer;
public class Install
{
    private readonly string _appFolder = AppConstants.AppDirectory;
    private readonly AppAuthenticationService _appAuthenticationService;
    private readonly IAuthenticationService _authenticationService;
    private readonly AppSessionService _sessionService;
    private readonly CloudDropDbContext _dbContext;

    //private readonly IServiceScopeFactory _scopeFactory;
    private CommandLineOptions _options;

    public Install(AppAuthenticationService appAuthenticationService,
        IAuthenticationService authenticationService,
        AppSessionService sessionService,
        CloudDropDbContext dbContext)
    {
        _appAuthenticationService = appAuthenticationService;
        _authenticationService = authenticationService;
        _sessionService = sessionService;
        _dbContext = dbContext;
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

            //using var scope = _scopeFactory.CreateScope();
            //using var context = scope.ServiceProvider.GetRequiredService<CloudDropDbContext>();
            _dbContext.Database.EnsureCreated();


            Console.WriteLine("Configuring...");

            // Set files directory
            string? filesDirectory = _options.FilesDirectory ?? AskForFilesDirectory();

SetApiUrl:
// Set Api Url
            string apiUrl = _options.ApiUrl ?? AskForApiUrl()
                ?? throw new Exception("Api Url must be set");
            apiUrl += apiUrl[^1] != '/' ? "/" : "";

            //bool isValid = ValidateUrlWithRegex(apiUrl);
            //if (isValid)
            //{
            //    Console.WriteLine($"Invalid Api url {Environment.NewLine}");

            //    // Reset supplied options so application can ask for new one
            //    _options.ApiUrl = null;

            //    goto SetApiUrl;
            //}

            int tries = 3;
SetUsernamePassword:
            if (--tries == 0)
            {
                return 0;
            }
            // Set username/email and password
            // Authenticate User
            string? username = _options.Username ?? AskForUsernameOrEmail();
            string? password = _options.Password ?? AskForPassword();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine($"Invalid username or password {Environment.NewLine}");
                goto SetUsernamePassword;
            }

            Console.WriteLine("Authenticating user...");
            var user = await _appAuthenticationService.AuthenticateUserAsync(apiUrl, username, password);
            if (user is null || !user.IsAuthenticated || user.Data is null)
            {
                Console.WriteLine($"Invalid username or password {Environment.NewLine}");
                goto SetUsernamePassword;
            }

            var dto = new AuthenticationDto()
            {
                AuthToken = user.Data.AccessToken.Token,
                ApiUrl = apiUrl,
                FilesDirectory = filesDirectory,
                Username = username,
            };
            bool isUpdated = await _authenticationService.AddOrUpdateAsync(dto);

            if (!isUpdated)
            {
                Console.WriteLine("Error unable to set data");
                return 1;
            }
            _sessionService.SetSession(dto);

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

    private string? AskForApiUrl()
    {
        return Prompt.GetString("What is the API URL? :",
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