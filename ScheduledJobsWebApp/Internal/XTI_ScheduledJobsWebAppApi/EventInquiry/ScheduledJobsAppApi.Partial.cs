using XTI_ScheduledJobsWebAppApi.EventInquiry;

namespace XTI_ScheduledJobsWebAppApi;

partial class ScheduledJobsAppApi
{
    private EventInquiryGroup? _EventInquiry;

    public EventInquiryGroup EventInquiry { get => _EventInquiry ?? throw new ArgumentNullException(nameof(_EventInquiry)); }

    partial void createEventInquiryGroup(IServiceProvider sp)
    {
        _EventInquiry = new EventInquiryGroup
        (
            source.AddGroup(nameof(EventInquiry)),
            sp
        );
    }
}