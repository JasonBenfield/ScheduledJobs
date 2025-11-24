import { EventNotification } from "./EventNotification";
import { SourceLogEntry } from "./SourceLogEntry";
import { TriggeredJob } from "./TriggeredJob";
import { TriggeredJobTask } from "./TriggeredJobTask";

export class TriggeredJobDetail {
    readonly job: TriggeredJob;
    readonly triggeredBy: EventNotification;
    readonly tasks: TriggeredJobTask[];
    readonly sourceLogEntries: SourceLogEntry[];

    constructor(source?: ITriggeredJobDetailModel) {
        this.job = new TriggeredJob(source && source.Job);
        this.triggeredBy = new EventNotification(source && source.TriggeredBy);
        this.tasks = source ? source.Tasks.map(t => new TriggeredJobTask(t)) : [];
        this.sourceLogEntries = source ? source.SourceLogEntries.map(le => new SourceLogEntry(le)) : [];
    }
}