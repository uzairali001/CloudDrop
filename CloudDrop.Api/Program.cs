using Asp.Versioning;

using CloudDrop.Api;
using CloudDrop.Api.Core;
using CloudDrop.Api.Core.Contracts.Repositories;
using CloudDrop.Api.Core.Contracts.Services.Data;
using CloudDrop.Api.Core.Contracts.Services.General;
using CloudDrop.Api.Core.Entities;
using CloudDrop.Api.Core.Models.Settings;
using CloudDrop.Api.Core.Repositories;
using CloudDrop.Api.Core.Services.Data;
using CloudDrop.Api.Core.Services.General;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region Authentication
// Authentication
var authConfig = builder.Configuration
    .GetSection(AuthenticationSettings.SettingsKey)
    .Get<AuthenticationSettings>() ?? throw new Exception("Authentication config not found");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authConfig.Issuer,
        ValidAudience = authConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.SecretKey)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) =>
        {
            return notBefore <= DateTime.UtcNow &&
                   expires > DateTime.UtcNow;
        }
    };
});
#endregion

#region Versionning
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
    config.ApiVersionReader = ApiVersionReader.Combine(
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("ver"),
        new UrlSegmentApiVersionReader()
    );
})
.AddMvc()
.AddApiExplorer(options =>
{
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
    // note: the specified format code will format the version as "'v'major[.minor][-status]"
    options.GroupNameFormat = "'v'VVV";

    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    options.SubstituteApiVersionInUrl = true;
});
#endregion

#region Database
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new Exception("Connection string not found");

builder.Services.AddDbContext<CloudDropDbContext>(options => options
    .UseMySql(connectionString, ServerVersion.Parse("8.0.28"), x =>
    {
        x.MigrationsAssembly("CloudDrop.Api.Core");
    })
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);
#endregion


#region Swagger
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerOperationFilter>();

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
#endregion

#region Dependency Injection

// Mapper
builder.Services.AddAutoMapper(typeof(PlaceholderForAutoMapper));

// Services - Data
builder.Services.AddTransient<IUploadSessionService, UploadSessionService>();

// Services - General
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

// Repositories
builder.Services.AddTransient<IUploadSessionRepository, UploadSessionRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IFileRepository, FileRepository>();
#endregion 


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
