using XTI_HubAppClient.WebApp.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using ScheduledJobsWebApp.ApiControllers;
using XTI_Core;
using XTI_ScheduledJobsWebAppApi;
using XTI_App.Api;

var builder = XtiWebAppHost.CreateDefault(ScheduledJobsInfo.AppKey, args);
var xtiEnv = XtiEnvironment.Parse(builder.Environment.EnvironmentName);
builder.Services.ConfigureXtiCookieAndTokenAuthentication(xtiEnv, builder.Configuration);
builder.Services.AddScoped<AppApiFactory, ScheduledJobsAppApiFactory>();
builder.Services.AddScoped(sp => (ScheduledJobsAppApi)sp.GetRequiredService<IAppApi>());
builder.Services.AddScheduledJobsAppApiServices();
builder.Services
    .AddMvc()
    .AddJsonOptions(options =>
    {
        options.SetDefaultJsonOptions();
    })
    .AddMvcOptions(options =>
    {
        options.SetDefaultMvcOptions();
    });
builder.Services.AddControllersWithViews()
    .PartManager.ApplicationParts.Add
    (
        new AssemblyPart(typeof(HomeController).Assembly)
    );

var app = builder.Build();
app.UseXtiDefaults();
await app.RunAsync();