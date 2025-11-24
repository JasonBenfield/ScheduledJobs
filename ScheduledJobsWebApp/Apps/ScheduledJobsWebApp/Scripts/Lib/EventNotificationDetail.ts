import { EventNotification } from "./EventNotification";
import { JobSummary } from "./JobSummary";

export class EventNotificationDetail {
    readonly event: EventNotification;
    readonly triggeredJobs: JobSummary[];

    constructor(source?: IEventNotificationDetailModel) {
        this.event = new EventNotification(source && source.Event);
        this.triggeredJobs = source ? source.TriggeredJobs.map(j => new JobSummary(j)) : [];
    }
}