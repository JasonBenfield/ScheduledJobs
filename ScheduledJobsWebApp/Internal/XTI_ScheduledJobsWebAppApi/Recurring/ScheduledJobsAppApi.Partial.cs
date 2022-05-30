using XTI_ScheduledJobsWebAppApi.Recurring;

namespace XTI_ScheduledJobsWebAppApi;

partial class ScheduledJobsAppApi
{
    private RecurringGroup? _Recurring;

    public RecurringGroup Recurring { get => _Recurring ?? throw new ArgumentNullException(nameof(_Recurring)); }

    partial void createRecurringGroup(IServiceProvider sp)
    {
        _Recurring = new RecurringGroup
        (
            source.AddGroup(nameof(Recurring)),
            sp
        );
    }
}