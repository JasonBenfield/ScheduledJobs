import { BasicComponent } from "@jasonbenfield/sharedwebapp/Components/BasicComponent";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { TriggeredJobTask } from "../../../Lib/TriggeredJobTask";
import { TaskListItemView } from "./TaskListItemView";

export class TaskListItem extends BasicComponent {
    constructor(readonly task: TriggeredJobTask, view: TaskListItemView) {
        super(view);
        let generation = "";
        if (task.generation > 1) {
            for (let i = 0; i < task.generation - 1; i++) {
                generation += ">>";
            }
        }
        view.generation.setText(generation);
        const displayTextTextComponent = this.addComponent(new TextComponent(view.displayText));
        const timeStartedTextComponent = this.addComponent(new TextComponent(view.timeStarted));
        const timeElapsedTextComponent = this.addComponent(new TextComponent(view.timeElapsed));
        const statusTextComponent = this.addComponent(new TextComponent(view.status));
        const logEntryCountTextComponent = this.addComponent(new TextComponent(view.logEntryCount));
        displayTextTextComponent.setText(task.taskDefinition.taskKey.displayText);
        timeStartedTextComponent.setText(
            task.timeStarted.isMaxYear ?
                "" :
                task.timeStarted.format()
        );
        timeElapsedTextComponent.setText(
            task.timeStarted.isMaxYear || task.timeEnded.isMaxYear ?
                "" :
                task.timeEnded.minus(task.timeStarted).format()
        );
        if (task.isFailed) {
            view.failed();
        }
        statusTextComponent.setText(task.status.DisplayText);
        if (task.logEntries.length > 0) {
            logEntryCountTextComponent.setText(task.logEntries.length.toString());
            view.logEntryCount.show();
        }
        else {
            view.logEntryCount.hide();
        }
    }
}