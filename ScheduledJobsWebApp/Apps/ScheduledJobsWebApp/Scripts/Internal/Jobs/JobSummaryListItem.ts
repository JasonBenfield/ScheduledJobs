import { BasicComponent } from "@jasonbenfield/sharedwebapp/Components/BasicComponent";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { ScheduledJobsAppClient } from "../../Lib/Http/ScheduledJobsAppClient";
import { JobSummaryListItemView } from "./JobSummaryListItemView";

export class JobSummaryListItem extends BasicComponent {
    constructor(schdJobsClient: ScheduledJobsAppClient, job: IJobSummaryModel, itemView: JobSummaryListItemView) {
        super(itemView);
        itemView.setHref(schdJobsClient.JobInquiry.JobDetail.getUrl({ JobID: job.ID }).value());
        new TextComponent(itemView.displayText).setText(job.JobKey.DisplayText);
        new TextComponent(itemView.status).setText(job.Status.DisplayText);
        new TextComponent(itemView.timeStarted).setText(
            job.TimeStarted.isMaxYear ? '' : job.TimeStarted.format()
        );
        new TextComponent(itemView.timeElapsed).setText(
            job.TimeStarted.isMaxYear || job.TimeEnded.isMaxYear ?
                '' :
                job.TimeEnded.minus(job.TimeStarted).format()
        );
        new TextComponent(itemView.taskCount).setText(job.TaskCount.toString());
    }
}