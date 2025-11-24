import { DateTimeOffset } from "@jasonbenfield/sharedwebapp/Common";
import { EventDefinition } from "./EventDefinition";
export class EventNotification {
    readonly id: number;
    readonly definition: EventDefinition;
    readonly sourceKey: string;
    readonly sourceData: string;
    readonly timeAdded: DateTimeOffset;
    readonly timeActive: DateTimeOffset;
    readonly timeInactive: DateTimeOffset;

    constructor(source?: IEventNotificationModel) {
        this.id = source ? source.ID : 0;
        this.definition = new EventDefinition(source && source.Definition);
        this.sourceKey = source ? source.SourceKey : "";
        this.sourceData = source ? source.SourceData : "";
        this.timeAdded = source ? source.TimeAdded : DateTimeOffset.max();
        this.timeInactive = source ? source.TimeInactive : DateTimeOffset.max();
    }
}