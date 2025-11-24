import { DateTimeOffset } from "@jasonbenfield/sharedwebapp/Common";
import { JobTaskStatus } from "./Http/JobTaskStatus";
import { JobKey } from "./JobKey";

export class JobSummary {
    readonly id: number;
    readonly jobKey: JobKey;
    readonly status: JobTaskStatus;
    readonly timeStarted: DateTimeOffset;
    readonly timeEnded: DateTimeOffset;
    readonly taskCount: number;

    constructor(source?: IJobSummaryModel) {
        this.id = source ? source.ID : 0;
        this.jobKey = new JobKey(source && source.JobKey);
        this.status = source ? JobTaskStatus.values.value(source.Status) : JobTaskStatus.values.NotSet;
        this.timeStarted = source ? source.TimeStarted : DateTimeOffset.max();
        this.timeEnded = source ? source.TimeEnded : DateTimeOffset.max();
    }
}