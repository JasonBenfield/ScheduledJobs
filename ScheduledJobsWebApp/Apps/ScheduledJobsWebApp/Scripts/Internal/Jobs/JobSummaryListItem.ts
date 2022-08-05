import { FormattedDate } from "@jasonbenfield/sharedwebapp/FormattedDate";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { ScheduledJobsAppApi } from "../../Lib/Api/ScheduledJobsAppApi";
import { FormattedTimeSpan } from "../FormattedTimeSpan";
import { JobSummaryListItemView } from "./JobSummaryListItemView";
import { BasicComponent } from "@jasonbenfield/sharedwebapp/Components/BasicComponent";

export class JobSummaryListItem extends BasicComponent {
    constructor(schdJobsApp: ScheduledJobsAppApi, job: IJobSummaryModel, itemView: JobSummaryListItemView) {
        super(itemView);
        itemView.setHref(schdJobsApp.JobInquiry.JobDetail.getUrl({ JobID: job.ID }).value());
        new TextComponent(itemView.displayText).setText(job.JobKey.DisplayText);
        new TextComponent(itemView.status).setText(job.Status.DisplayText);
        new TextComponent(itemView.timeStarted).setText(new FormattedDate(job.TimeStarted).formatDateTime());
        new TextComponent(itemView.timeElapsed).setText(new FormattedTimeSpan(job.TimeStarted, job.TimeEnded).format());
        new TextComponent(itemView.taskCount).setText(job.TaskCount.toString());
    }
}