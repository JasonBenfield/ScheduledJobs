// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class ScheduledJobsAppClientFactory
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly XtiTokenAccessorFactory xtiTokenAccessorFactory;
    private readonly AppClientUrl clientUrl;
    private readonly IAppClientRequestKey requestKey;
    private readonly ScheduledJobsAppClientVersion version;
    public ScheduledJobsAppClientFactory(IHttpClientFactory httpClientFactory, XtiTokenAccessorFactory xtiTokenAccessorFactory, AppClientUrl clientUrl, IAppClientRequestKey requestKey, ScheduledJobsAppClientVersion version)
    {
        this.httpClientFactory = httpClientFactory;
        this.xtiTokenAccessorFactory = xtiTokenAccessorFactory;
        this.clientUrl = clientUrl;
        this.requestKey = requestKey;
        this.version = version;
    }

    public ScheduledJobsAppClient Create() => new ScheduledJobsAppClient(httpClientFactory, xtiTokenAccessorFactory, clientUrl, requestKey, version);
}