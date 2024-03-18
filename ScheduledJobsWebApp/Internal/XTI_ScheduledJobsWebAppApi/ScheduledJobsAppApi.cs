namespace XTI_ScheduledJobsWebAppApi;

public sealed partial class ScheduledJobsAppApi : WebAppApiWrapper
{
    public ScheduledJobsAppApi
    (
        IAppApiUser user,
        IServiceProvider sp
    )
        : base
        (
            new AppApi
            (
                ScheduledJobsInfo.AppKey,
                user,
                ResourceAccess.AllowAuthenticated()
                    .WithAllowed(AppRoleName.Admin),
                ""
            ),
            sp
        )
    {
        createHomeGroup(sp);
        createRecurringGroup(sp);
        createEventDefinitionsGroup(sp);
        createEventInquiryGroup(sp);
        createEventsGroup(sp);
        createJobDefinitionsGroup(sp);
        createJobInquiryGroup(sp);
        createJobsGroup(sp);
        createTasksGroup(sp);
    }

    partial void createHomeGroup(IServiceProvider sp);

    partial void createRecurringGroup(IServiceProvider sp);

    partial void createEventDefinitionsGroup(IServiceProvider sp);

    partial void createEventInquiryGroup(IServiceProvider sp);

    partial void createEventsGroup(IServiceProvider sp);

    partial void createJobDefinitionsGroup(IServiceProvider sp);

    partial void createJobInquiryGroup(IServiceProvider sp);

    partial void createJobsGroup(IServiceProvider sp);

    partial void createTasksGroup(IServiceProvider sp);

    protected override void ConfigureTemplate(AppApiTemplate template)
    {
        base.ConfigureTemplate(template);
        template.ExcludeValueTemplates
        (
            (temp, generators) =>
            {
                if (generators == ApiCodeGenerators.Dotnet)
                {
                    var ns = temp.DataType.Namespace ?? "";
                    return ns.StartsWith("XTI_Jobs.Abstractions") || ns.StartsWith("XTI_Hub.Abstractions");
                }
                return false;
            }
        );
    }
}