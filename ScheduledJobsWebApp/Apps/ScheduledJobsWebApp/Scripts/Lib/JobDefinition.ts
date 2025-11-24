import { JobKey } from "./JobKey";

export class JobDefinition {
    readonly id: number;
    readonly jobKey: JobKey;

    constructor(source?: IJobDefinitionModel) {
        this.id = source ? source.ID : 0;
        this.jobKey = new JobKey(source && source.JobKey);
    }
}