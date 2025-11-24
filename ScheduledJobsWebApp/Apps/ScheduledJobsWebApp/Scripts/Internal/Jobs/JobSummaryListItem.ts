import { BasicComponent } from "@jasonbenfield/sharedwebapp/Components/BasicComponent";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { ScheduledJobsAppClient } from "../../Lib/Http/ScheduledJobsAppClient";
import { JobSummary } from "../../Lib/JobSummary";
import { JobSummaryListItemView } from "./JobSummaryListItemView";

export class JobSummaryListItem extends BasicComponent {
    constructor(schdJobsClient: ScheduledJobsAppClient, job: JobSummary, itemView: JobSummaryListItemView) {
        super(itemView);
        itemView.setHref(schdJobsClient.JobInquiry.JobDetail.getUrl({ JobID: job.id }).value());
        const displayTextTextComponent = this.addComponent(new TextComponent(itemView.displayText));
        const statusTextComponent = this.addComponent(new TextComponent(itemView.status));
        const timeStartedTextComponent = this.addComponent(new TextComponent(itemView.timeStarted));
        const timeElapsedTextComponent = this.addComponent(new TextComponent(itemView.timeElapsed));
        const taskCountTextComponent = this.addComponent(new TextComponent(itemView.taskCount));
        displayTextTextComponent.setText(job.jobKey.displayText);
        statusTextComponent.setText(job.status.DisplayText);
        timeStartedTextComponent.setText(
            job.timeStarted.isMaxYear ? "" : job.timeStarted.format()
        );
        timeElapsedTextComponent.setText(
            job.timeStarted.isMaxYear || job.timeEnded.isMaxYear ?
                "" :
                job.timeEnded.minus(job.timeStarted).format()
        );
        taskCountTextComponent.setText(job.taskCount.toString());
    }
}