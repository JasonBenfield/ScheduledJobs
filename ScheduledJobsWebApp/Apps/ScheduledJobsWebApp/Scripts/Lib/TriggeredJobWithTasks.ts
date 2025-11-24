import { TriggeredJob } from "./TriggeredJob";
import { TriggeredJobTask } from "./TriggeredJobTask";

export class TriggeredJobWithTasks {
    readonly job: TriggeredJob;
    readonly tasks: TriggeredJobTask[];

    constructor(source?: ITriggeredJobWithTasksModel) {
        this.job = new TriggeredJob(source && source.Job);
        this.tasks = source ? source.Tasks.map(t => new TriggeredJobTask(t)) : [];
    }
}