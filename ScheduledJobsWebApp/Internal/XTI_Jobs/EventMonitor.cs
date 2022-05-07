using XTI_Core;

namespace XTI_Jobs;

public sealed class EventMonitor
{
    private readonly IStoredEvents storedEvents;
    private readonly IClock clock;
    private readonly EventKey eventKey;
    private readonly JobKey jobKey;

    internal EventMonitor(IStoredEvents storedEvents, IClock clock, EventKey eventKey, JobKey jobKey)
    {
        this.storedEvents = storedEvents;
        this.clock = clock;
        this.eventKey = eventKey;
        this.jobKey = jobKey;
    }

    public async Task Run()
    {
        var triggeredJobs = await storedEvents.TriggerJobs(eventKey, jobKey, clock.Now());
        foreach(var triggeredJob in triggeredJobs) 
        { 
        }
    }
}
