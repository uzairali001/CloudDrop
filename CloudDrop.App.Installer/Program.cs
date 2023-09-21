using CloudDrop.App.Core.Contracts.Repositories;
using CloudDrop.App.Core.Contracts.Services.Data;
using CloudDrop.App.Core.Entities;
using CloudDrop.App.Core.Models.Options;
using CloudDrop.App.Core.Repositories;
using CloudDrop.App.Core.Services.Data;
using CloudDrop.App.Core.Services.General;
using CloudDrop.App.Installer.Constants;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.DependencyInjection;

using Serilog;

using UzairAli.NetHttpClient.Extensions;

namespace CloudDrop.App.Installer;
[Command(Name = "CloudDropInstaller", Description = "Install CloudDrop Client Application."),
    Subcommand(typeof(Install)),
    Subcommand(typeof(Uninstall))
    ]
public class Program
{
    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        var services = new ServiceCollection()
            .AddLogging(configure => configure.AddSerilog(dispose: true))
            .AddDbContext<CloudDropDbContext>()
            .AddScoped<AppAuthenticationService>()
            .AddSingleton<AppSessionService>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<IAuthenticationRepository, AuthenticationRepository>()
            .AddScoped<Install>()
            .AddScoped<Uninstall>()
            .AddHttpClientService(
            httpOptions => { },
            jsonOptions =>
            {
                jsonOptions.PropertyNameCaseInsensitive = true;
                jsonOptions.TypeInfoResolver = SourceGenerators.SourceGenerationContext.Default;
            })
            .BuildServiceProvider();


        // Description of the application
        CommandLineApplication app = new()
        {
            Name = "CloudDrop.exe",
            FullName = "CloudDrop",
            Description = "The client application for CloudDrop",
        };
        app.Conventions
            .UseConstructorInjection(services);


        // Handle help and version arguments
        // You can declare alias using "|"
        app.HelpOption("-?|-h|--help");
        app.VersionOption("--version", "1.0.0");

        // Plug the execution method
        app.OnExecuteAsync(async (cancelation) =>
        {
            app.ShowHelp();
            return 0;
        });

        app.Command("install", cmd =>
        {
            cmd.HelpOption("-?|-h|--help");
            cmd.Description = "Install the app";

            CommandOption apiUrl = cmd.Option($"--{CliConstants.ApiUrl}", "Url for API", CommandOptionType.SingleValue);
            CommandOption username = cmd.Option($"--{CliConstants.Username}", "User's username or email", CommandOptionType.SingleValue);
            CommandOption password = cmd.Option($"--{CliConstants.Password}", "User's password", CommandOptionType.SingleValue);
            CommandOption targetDirectory = cmd.Option($"--{CliConstants.TargetDirectory}", "The directory of the files to upload.", CommandOptionType.SingleValue);
            CommandOption run = cmd.Option($"--{CliConstants.Run}", "Run the app immediately after installation", CommandOptionType.NoValue);

            cmd.OnExecuteAsync(ct =>
            {
                CommandLineOptions options = new()
                {
                    ApiUrl = apiUrl.Value(),
                    Username = username.Value(),
                    Password = password.Value(),
                    FilesDirectory = targetDirectory.Value(),
                    RunImmediately = run.HasValue(),
                };
                return cmd.GetRequiredService<Install>().OnExecuteAsync(options);
            });
        });
        app.Command("uninstall", cmd =>
        {
            cmd.HelpOption("-?|-h|--help");
            cmd.Description = "Uninstall the app";

            CommandOption noPrompt = cmd.Option($"-{CliConstants.NoPrompt}", "Don't ask for confirmation", CommandOptionType.NoValue);

            cmd.OnExecuteAsync(ct =>
            {
                Dictionary<string, CommandOption>? options =
                    noPrompt.HasValue()
                    ? new()
                        {
                            { CliConstants.NoPrompt, noPrompt },
                        }
                    : null;
                return cmd.GetRequiredService<Uninstall>().OnExecuteAsync(options);
            });
        });

        // Parse the command line and execute the right code
        try
        {
            await app.ExecuteAsync(args);
        }
        catch (CommandParsingException ex)
        {
            Console.WriteLine(ex.Message);
            app.ShowHelp();
        }

    }
}