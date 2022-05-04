using XTI_HubAppClient.WebApp.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using ScheduleJobWebApp.ApiControllers;
using XTI_Core;
using XTI_ScheduleJobWebAppApi;
using XTI_App.Api;

var builder = XtiWebAppHost.CreateDefault(ScheduleJobInfo.AppKey, args);
var xtiEnv = XtiEnvironment.Parse(builder.Environment.EnvironmentName);
builder.Services.ConfigureXtiCookieAndTokenAuthentication(xtiEnv, builder.Configuration);
builder.Services.AddScoped<AppApiFactory, ScheduleJobAppApiFactory>();
builder.Services.AddScoped(sp => (ScheduleJobAppApi)sp.GetRequiredService<IAppApi>());
builder.Services.AddScheduleJobAppApiServices();
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