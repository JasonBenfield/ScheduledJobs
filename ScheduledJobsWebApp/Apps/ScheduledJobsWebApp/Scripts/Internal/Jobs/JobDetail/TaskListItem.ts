import { BasicComponent } from "@jasonbenfield/sharedwebapp/Components/BasicComponent";
import { FormattedDate } from "@jasonbenfield/sharedwebapp/FormattedDate";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { JobTaskStatus } from "../../../Lib/Api/JobTaskStatus";
import { FormattedTimeSpan } from "../../FormattedTimeSpan";
import { TaskListItemView } from "./TaskListItemView";

export class TaskListItem extends BasicComponent {
    constructor(readonly task: ITriggeredJobTaskModel, view: TaskListItemView) {
        super(view);
        let generation = '';
        if (task.Generation > 1) {
            for (let i = 0; i < task.Generation - 1; i++) {
                generation += '&nbsp;&nbsp;';
            }
        }
        view.generation.setText(generation);
        new TextComponent(view.displayText).setText(task.TaskDefinition.TaskKey.DisplayText);
        new TextComponent(view.timeStarted).setText(
            task.TimeStarted.getFullYear() < 9999 ?
                new FormattedDate(task.TimeStarted).formatDateTime() :
                ''
        );
        new TextComponent(view.timeElapsed).setText(new FormattedTimeSpan(task.TimeStarted, task.TimeEnded).format());
        let status = JobTaskStatus.values.value(task.Status.Value);
        if (status.equals(JobTaskStatus.values.Failed)) {
            view.failed();
        }
        new TextComponent(view.status).setText(task.Status.DisplayText);
        if (task.LogEntries.length > 0) {
            new TextComponent(view.logEntryCount).setText(task.LogEntries.length.toString());
            view.logEntryCount.show();
        }
        else {
            view.logEntryCount.hide();
        }
    }
}