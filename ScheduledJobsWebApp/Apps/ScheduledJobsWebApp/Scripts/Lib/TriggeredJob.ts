import { JobDefinition } from "./JobDefinition";

export class TriggeredJob {
    readonly id: number;
    readonly jobDefinition: JobDefinition;
    readonly eventNotificationID: number;

    constructor(source?: ITriggeredJobModel) {
        this.id = source ? source.ID : 0;
        this.jobDefinition = new JobDefinition(source && source.JobDefinition);
        this.eventNotificationID = source ? source.EventNotificationID : 0;
    }
}