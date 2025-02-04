using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SERP.Api.Common;
using SERP.Api.Common.Exceptions;
using SERP.Api.Common.FileServers;
using SERP.Application;
using SERP.Application.Common.BackendSchedulerTask.Services;
using SERP.Application.Masters.ApplicationTokens;
using SERP.Infrastructure;
using SERP.Infrastructure.Common.DBContexts;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region appsettings
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();
#endregion

// Add services to the container.

#region Cors
const string allowSpecificOrigins = "AllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyHeader()
                .AllowCredentials()
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true) // allow any origin
                .WithExposedHeaders("*");
        });
});
#endregion

#region Database Connectivity
if (!bool.TryParse((builder.Configuration["DbContext:ShowSql"]), out var showSql))
{
    showSql = false;
}

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"));
    if (showSql)
    {
        option.EnableSensitiveDataLogging(showSql);
        option.UseLoggerFactory(LoggerFactory.Create(config => config.AddConsole()));
    }
});
#endregion

#region Hangfire Database Connectivity
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

if (!int.TryParse(builder.Configuration.GetSection("HangfireSettings:WorkerCount").Value, out var workerCount))
{
    workerCount = 1;
}

builder.Services.AddHangfireServer(option => option.WorkerCount = workerCount);
#endregion

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IHostedService, CronJobService>();

builder.Services.AddScoped<HttpContextService>();

#region Include the services from Application and Infrastructure
const int maxFileSize = 524288000; // 500MB

// If using IIS
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = maxFileSize;
});
// If using Kestrel
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = maxFileSize;
});
builder.Services.Configure<FormOptions>(x =>
{
    x.ValueCountLimit = int.MaxValue;
    x.ValueLengthLimit = maxFileSize;
    x.MultipartBodyLengthLimit = maxFileSize;
    x.MultipartHeadersLengthLimit = maxFileSize;
});

builder.Services.Configure<BoldReportsSettings>(builder.Configuration.GetSection("BoldsReport"));

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger());

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddExceptionServices();
builder.Services.AddHttpClient();

builder.Services.Configure<CronJobSettings>(builder.Configuration.GetSection("CronJobSettings"));

#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Hangfire Job Demo Application",
    DarkModeEnabled = false,
    DisplayStorageConnectionString = false,
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter
        {
            User = builder.Configuration.GetSection("HangfireSettings:Username").Value,
            Pass = builder.Configuration.GetSection("HangfireSettings:Password").Value
        }
    }
});

#region DB Migrate
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    if (context.Database.IsSqlServer())
    {
        context.Database.Migrate();
    }
}
#endregion
app.UseRouting();
app.UseCors(allowSpecificOrigins);

app.UseHttpsRedirection();
FileServerHelper.ConfigureFileProviders(app, builder.Configuration);

app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();
app.ConfigureException();


app.Run();