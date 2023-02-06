using XTI_App.Abstractions;
using XTI_ScheduledJobsWebAppApi;
using XTI_WebApp.Api;

namespace ScheduledJobsWebApp;

internal sealed class SchdJobsMenuDefinitionBuilder : IMenuDefinitionBuilder
{
    private readonly UserMenuDefinition userMenuDefinition;
    private readonly ScheduledJobsAppApi api;

    public SchdJobsMenuDefinitionBuilder(UserMenuDefinition userMenuDefinition, ScheduledJobsAppApi api)
    {
        this.userMenuDefinition = userMenuDefinition;
        this.api = api;
    }

    public AppMenuDefinitions Build() =>
        new AppMenuDefinitions
        (
            userMenuDefinition.Value,
            new MenuDefinition
            (
                "main",
                new LinkModel("eventDefinitions", "Event Definitions", api.EventDefinitions.Index.Path.RootPath()),
                new LinkModel("eventNotifications", "Event Notifications", api.EventInquiry.Notifications.Path.RootPath()),
                new LinkModel("jobDefinitions", "Job Definitions", api.JobDefinitions.Index.Path.RootPath()),
                new LinkModel("failedJobs", "Failed Jobs", api.JobInquiry.FailedJobs.Path.RootPath()),
                new LinkModel("recentJobs", "Recent Jobs", api.JobInquiry.RecentJobs.Path.RootPath())
            )
        );
}


