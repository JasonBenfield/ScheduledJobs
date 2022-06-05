import { FormattedDate } from "@jasonbenfield/sharedwebapp/FormattedDate";
import { TextBlock } from "@jasonbenfield/sharedwebapp/Html/TextBlock";
import { ScheduledJobsAppApi } from "../../../ScheduledJobs/Api/ScheduledJobsAppApi";
import { FormattedTimeSpan } from "../FormattedTimeSpan";
import { JobSummaryListItemView } from "./JobSummaryListItemView";

export class JobSummaryListItem {
    constructor(schdJobsApp: ScheduledJobsAppApi, job: IJobSummaryModel, itemView: JobSummaryListItemView) {
        itemView.setHref(schdJobsApp.JobInquiry.JobDetail.getUrl({ JobID: job.ID }).value());
        new TextBlock(job.JobKey.DisplayText, itemView.displayText);
        new TextBlock(job.Status.DisplayText, itemView.status);
        new TextBlock(new FormattedDate(job.TimeStarted).formatDateTime(), itemView.timeStarted);
        new TextBlock(new FormattedTimeSpan(job.TimeStarted, job.TimeEnded).format(), itemView.timeElapsed);
        new TextBlock(job.TaskCount.toString(), itemView.taskCount);
    }
}