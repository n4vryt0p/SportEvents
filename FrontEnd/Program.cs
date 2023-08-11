using FrontEnd.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddJsonFile("samlsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile("privilegesettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings-{builder.Environment.ApplicationName}.json", optional: true, reloadOnChange: true);

_ = builder.WebHost.ConfigureKestrel((context, options) =>
{
    // Handle requests up to 50 MB
    options.Limits.MaxRequestBodySize = 52428800;
});

var cfgs = builder.Configuration;
builder.Services.RegisterDiServices(cfgs, null);

using var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.AppConfigurations();

app.Run();

public partial class Program { }