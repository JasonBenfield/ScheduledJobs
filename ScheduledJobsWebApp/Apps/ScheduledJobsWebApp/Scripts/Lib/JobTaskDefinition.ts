import { JobTaskKey } from "./JobTaskKey";

export class JobTaskDefinition {
    readonly id: number;
    readonly taskKey: JobTaskKey;

    constructor(source?: IJobTaskDefinitionModel) {
        this.id = source ? source.ID : 0;
        this.taskKey = new JobTaskKey(source && source.TaskKey);
    }
}