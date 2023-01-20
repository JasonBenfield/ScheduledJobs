using XTI_App.Abstractions;
using XTI_Core;
using XTI_Core.Extensions;
using XTI_DB;
using XTI_Hub.Abstractions;
using XTI_HubAppClient;
using XTI_JobsDB.EF;
using XTI_ScheduledJobsWebAppApi;

namespace XTI_ScheduledJobIntegrationTests;

internal static class XtiTestHostExtensions
{
    public static async Task Setup(this XtiHost host)
    {
        var dbAdmin = host.GetRequiredService<DbAdmin<JobDbContext>>();
        await dbAdmin.Update();
        var xtiEnv = host.GetRequiredService<XtiEnvironment>();
        if (xtiEnv.IsTest())
        {
            await dbAdmin.Reset();
        }
        var hubClient = host.GetRequiredService<HubAppClient>();
        var adminUserCreds = host.GetRequiredService<AdminUserCredentials>();
        await hubClient.Users.AddOrUpdateUser
        (
            "General",
            new AddOrUpdateUserRequest
            {
                UserName = adminUserCreds.Value.UserName,
                Password = adminUserCreds.Value.Password,
                PersonName = "Scheduled Jobs Admin"
            }
        );
        await hubClient.Install.SetUserAccess
        (
            new SetUserAccessRequest
            (
                new AppUserName(adminUserCreds.Value.UserName),
                new SetUserAccessRoleRequest
                (
                    ScheduledJobsInfo.AppKey,
                    new AppRoleName(hubClient.RoleNames.Admin)
                )
            )
        );
        hubClient.UseToken<AdminUserXtiToken>();
    }

    public static async Task Register(this XtiHost host, Action<EventRegistration> configEvents, Action<JobRegistration> configJobs)
    {
        var events = host.GetRequiredService<EventRegistration>();
        configEvents(events);
        await events.Register();
        var jobs = host.GetRequiredService<JobRegistration>();
        configJobs(jobs);
        await jobs.Register();
    }

    public static Task MonitorEvent(this XtiHost host, EventKey eventKey, JobKey jobKey)
    {
        var stoppingToken = host.GetRequiredService<CancellationTokenSource>().Token;
        var monitorBuilder = host.GetRequiredService<EventMonitorBuilder>();
        var jobActionFactory = host.GetRequiredService<DemoJobActionFactory>();
        var transformedEventData = host.GetRequiredService<DemoTransformedEventData>();
        return monitorBuilder
            .When(eventKey)
            .Trigger(jobKey)
            .UseJobActionFactory(jobActionFactory)
            .TransformEventData(transformedEventData)
            .Run(stoppingToken);
    }

    public static Task<EventNotification[]> RaiseEvent(this XtiHost host, EventKey eventKey, XtiEventSource source)
    {
        var incomingEventFactory = host.GetRequiredService<IncomingEventFactory>();
        return incomingEventFactory
            .Incoming(eventKey)
            .From(source)
            .Notify();
    }
}
