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
                    .WithAllowed(AppRoleName.Admin)
            ),
            sp
        )
    {
        createHomeGroup(sp);
        createRecurringGroup(sp);
        createEventInquiryGroup(sp);
        createEventsGroup(sp);
        createJobInquiryGroup(sp);
        createJobsGroup(sp);
    }

    partial void createHomeGroup(IServiceProvider sp);

    partial void createRecurringGroup(IServiceProvider sp);

    partial void createEventInquiryGroup(IServiceProvider sp);

    partial void createEventsGroup(IServiceProvider sp);

    partial void createJobInquiryGroup(IServiceProvider sp);

    partial void createJobsGroup(IServiceProvider sp);

    protected override void ConfigureTemplate(AppApiTemplate template)
    {
        template.ExcludeValueTemplates
        (
            (temp, generators) =>
            {
                if(generators == ApiCodeGenerators.Dotnet)
                {
                    return temp.DataType.Namespace == "XTI_Jobs.Abstractions";
                }
                return false;
            }
        );
    }
}