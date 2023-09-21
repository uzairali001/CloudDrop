using CloudDrop.App.Core.Contracts.Repositories;
using CloudDrop.App.Core.Contracts.Services.Data;
using CloudDrop.App.Core.Entities;
using CloudDrop.App.Core.Repositories;
using CloudDrop.App.Core.Services.Data;
using CloudDrop.App.Core.Services.General;
using CloudDrop.App.Installer.SourceGenerators;
using CloudDrop.ConsoleApp;

using Serilog;

using UzairAli.NetHttpClient.Extensions;

Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

#if RELEASE
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
                .CreateLogger();
#else
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
#endif

Log.Logger.Information("Application Starting");


var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddDbContext<CloudDropDbContext>();
builder.Services.AddSingleton<AppSessionService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

builder.Services.AddHttpClientService(
    httpOptions => { },
    jsonOptions =>
    {
        jsonOptions.PropertyNameCaseInsensitive = true;
        jsonOptions.TypeInfoResolver = SourceGenerationContext.Default;
    });

var host = builder.Build();

using var scope = host.Services.CreateScope();
AppSessionService sessionService = scope.ServiceProvider.GetRequiredService<AppSessionService>();
IAuthenticationService authService = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();

var dto = await authService.GetAsync()
    ?? throw new Exception("Unable to get session");

sessionService.SetSession(dto);

if (!string.IsNullOrWhiteSpace(dto.FilesDirectory))
{
    Log.Logger.Information($"Target Directory: {dto.FilesDirectory}");
}

host.Run();
