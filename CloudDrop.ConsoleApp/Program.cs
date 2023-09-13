using CloudDrop.ConsoleApp;

using UzairAli.NetHttpClient.Extensions;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddHttpClientService(opts =>
{
    opts.BaseAddress = new Uri("http://localhost:5124/v1/");
});

var host = builder.Build();
host.Run();
