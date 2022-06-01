using XTI_App.Abstractions;
using XTI_Core;
using XTI_Core.Extensions;
using XTI_DB;
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
        var adminUserID = await hubClient.Users.AddOrUpdateUser
        (
            new AddUserModel
            {
                UserName = adminUserCreds.Value.UserName,
                Password = adminUserCreds.Value.Password,
                PersonName = "Scheduled Jobs Admin"
            }
        );
        await hubClient.Install.SetUserAccess
        (
            new SetUserAccessRequest
            {
                UserID = adminUserID,
                RoleAssignments = new[]
                {
                    new SetUserAccessRoleRequest
                    {
                        AppKey = ScheduledJobsInfo.AppKey,
                        RoleNames = new [] { new AppRoleName(hubClient.RoleNames.Admin) }
                    }
                }
            }
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
        var monitorFactory = host.GetRequiredService<EventMonitorFactory>();
        var monitor = monitorFactory
            .When(eventKey)
            .Trigger(jobKey);
        return monitor.Run(stoppingToken);
    }

    public static Task<EventNotification[]> RaiseEvent(this XtiHost host, EventKey eventKey, EventSource source)
    {
        var incomingEventFactory = host.GetRequiredService<IncomingEventFactory>();
        return incomingEventFactory
            .Incoming(eventKey)
            .From(source)
            .Notify();
    }
}
