import { BasicComponent } from "@jasonbenfield/sharedwebapp/Components/BasicComponent";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { JobTaskStatus } from "../../../Lib/Http/JobTaskStatus";
import { TaskListItemView } from "./TaskListItemView";

export class TaskListItem extends BasicComponent {
    constructor(readonly task: ITriggeredJobTaskModel, view: TaskListItemView) {
        super(view);
        let generation = '';
        if (task.Generation > 1) {
            for (let i = 0; i < task.Generation - 1; i++) {
                generation += '>>';
            }
        }
        view.generation.setText(generation);
        new TextComponent(view.displayText).setText(task.TaskDefinition.TaskKey.DisplayText);
        new TextComponent(view.timeStarted).setText(
            task.TimeStarted.isMaxYear ?
                '' :
                task.TimeStarted.format()
        );
        new TextComponent(view.timeElapsed).setText(
            task.TimeStarted.isMaxYear || task.TimeEnded.isMaxYear ?
                '' :
                task.TimeEnded.minus(task.TimeStarted).format()
        );
        const status = JobTaskStatus.values.value(task.Status.Value);
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