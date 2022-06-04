import { FormattedDate } from "@jasonbenfield/sharedwebapp/FormattedDate";
import { TextBlock } from "@jasonbenfield/sharedwebapp/Html/TextBlock";
import { JobSummaryListItemView } from "./JobSummaryListItemView";

export class JobSummaryListItem {
    constructor(job: IJobSummaryModel, itemView: JobSummaryListItemView) {
        new TextBlock(job.JobKey.DisplayText, itemView.displayText);
        new TextBlock(job.Status.DisplayText, itemView.status);
        new TextBlock(new FormattedDate(job.TimeStarted).formatDateTime(), itemView.timeStarted);
        let timeElapsedText = '';
        if (job.TimeEnded.getFullYear() < 9999) {
            let timeElapsed = job.TimeEnded.getTime() - job.TimeStarted.getTime();
            timeElapsedText = `${timeElapsed} ms`;
        }
        new TextBlock(timeElapsedText, itemView.timeElapsed);
        new TextBlock(job.TaskCount.toString(), itemView.taskCount);
    }
}