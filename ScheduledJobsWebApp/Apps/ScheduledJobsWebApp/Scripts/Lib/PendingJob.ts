import { TriggeredJob } from "./TriggeredJob";

export class PendingJob {
	readonly job: TriggeredJob;
	readonly sourceKey: string;
	readonly sourceData: string;

    constructor(source?: IPendingJobModel) {
		this.job = new TriggeredJob(source && source.Job);
		this.sourceKey = source ? source.SourceKey : "";
		this.sourceData = source ? source.SourceData : "";
    }
}