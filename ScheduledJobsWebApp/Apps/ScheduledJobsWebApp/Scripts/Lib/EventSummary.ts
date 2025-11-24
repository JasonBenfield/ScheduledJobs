import { EventNotification } from "./EventNotification";

export class EventSummary {
    readonly event: EventNotification;
    readonly triggeredJobCount: number;

    constructor(source?: IEventSummaryModel) {
        this.event = new EventNotification(source && source.Event);
        this.triggeredJobCount = source ? source.TriggeredJobCount : 0;
    }
}