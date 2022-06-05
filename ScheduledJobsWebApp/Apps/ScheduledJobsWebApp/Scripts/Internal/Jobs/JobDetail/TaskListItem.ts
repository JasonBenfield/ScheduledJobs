import { FormattedDate } from "@jasonbenfield/sharedwebapp/FormattedDate";
import { TextBlock } from "@jasonbenfield/sharedwebapp/Html/TextBlock";
import { FormattedTimeSpan } from "../FormattedTimeSpan";
import { TaskListItemView } from "./TaskListItemView";
import { JobTaskStatus } from "../../../ScheduledJobs/Api/JobTaskStatus";
import { ContextualClass } from "@jasonbenfield/sharedwebapp/ContextualClass";

export class TaskListItem {
    constructor(readonly task: ITriggeredJobTaskModel, view: TaskListItemView) {
        new TextBlock(task.TaskDefinition.TaskKey.DisplayText, view.displayText);
        new TextBlock(
            task.TimeStarted.getFullYear() < 9999 ?
                new FormattedDate(task.TimeStarted).formatDateTime() :
                '',
            view.timeStarted
        );
        new TextBlock(new FormattedTimeSpan(task.TimeStarted, task.TimeEnded).format(), view.timeElapsed);
        let status = JobTaskStatus.values.value(task.Status.Value);
        if (status.equals(JobTaskStatus.values.Failed)) {
            view.failed();
        }
        new TextBlock(task.Status.DisplayText, view.status);
        if (task.LogEntries.length > 0) {
            new TextBlock(task.LogEntries.length.toString(), view.logEntryCount);
            view.logEntryCount.show();
        }
        else {
            view.logEntryCount.hide();
        }
    }
}