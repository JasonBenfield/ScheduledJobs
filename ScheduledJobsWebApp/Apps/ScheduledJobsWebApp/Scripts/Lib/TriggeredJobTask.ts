import { DateTimeOffset } from "@jasonbenfield/sharedwebapp/Common";
import { JobTaskStatus } from "./Http/JobTaskStatus";
import { JobLogEntry } from "./JobLogEntry";
import { JobTaskDefinition } from "./JobTaskDefinition";

export class TriggeredJobTask {
    readonly id: number;
    readonly taskDefinition: JobTaskDefinition;
    readonly status: JobTaskStatus;
    readonly generation: number;
    readonly timeStarted: DateTimeOffset;
    readonly timeEnded: DateTimeOffset;
    readonly taskData: string;
    readonly logEntries: JobLogEntry[];

    constructor(source?: ITriggeredJobTaskModel) {
        this.id = source ? source.ID : 0;
        this.taskDefinition = new JobTaskDefinition(source && source.TaskDefinition);
        this.status = source ? JobTaskStatus.values.value(source.Status) : JobTaskStatus.values.NotSet;
        this.generation = source ? source.Generation : 0;
        this.timeStarted = source ? source.TimeStarted : DateTimeOffset.max();
        this.timeEnded = source ? source.TimeEnded : DateTimeOffset.max();
        this.taskData = source ? source.TaskData : "";
        this.logEntries = source ? source.LogEntries.map(le => new JobLogEntry(le)) : [];
    }

    get isFailed() { return this.status.equals(JobTaskStatus.values.Failed); }
    get isRetry() { return this.status.equals(JobTaskStatus.values.Retry); }
    get isSkip() { return this.status.equals(JobTaskStatus.values.Skip); }
    get isRunning() { return this.status.equals(JobTaskStatus.values.Running); }
    get isPending() { return this.status.equals(JobTaskStatus.values.Pending); }
    get isCanceled() { return this.status.equals(JobTaskStatus.values.Canceled); }
    get isCompleted() { return this.status.equals(JobTaskStatus.values.Completed); }
}