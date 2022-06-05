import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { Command } from "@jasonbenfield/sharedwebapp/Command/Command";
import { FormattedDate } from "@jasonbenfield/sharedwebapp/FormattedDate";
import { TextBlock } from "@jasonbenfield/sharedwebapp/Html/TextBlock";
import { ListGroup } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroup";
import { FormattedTimeSpan } from "../FormattedTimeSpan";
import { LogEntryItem } from "./LogEntryItem";
import { LogEntryItemView } from "./LogEntryItemView";
import { TaskDetailPanelView } from "./TaskDetailPanelView";

interface Results {
    backRequested?: {};
}

export class TaskDetailPanelResult {
    static get backRequested() { return new TaskDetailPanelResult({ backRequested: {} }); }

    private constructor(private readonly results: Results) { }

    get backRequested() { return this.results.backRequested; }
}

export class TaskDetailPanel implements IPanel {
    private readonly awaitable = new Awaitable<TaskDetailPanelResult>();
    private readonly displayText: TextBlock;
    private readonly status: TextBlock;
    private readonly timeStarted: TextBlock;
    private readonly timeElapsed: TextBlock;
    private readonly taskData: TextBlock;
    private readonly logEntries: ListGroup;
    private tasks: ITriggeredJobTaskModel[];
    private currentTask: ITriggeredJobTaskModel;

    constructor(private view: TaskDetailPanelView) {
        this.displayText = new TextBlock('', this.view.displayText);
        this.status = new TextBlock('', this.view.status);
        this.timeStarted = new TextBlock('', this.view.timeStarted);
        this.timeElapsed = new TextBlock('', this.view.timeElapsed);
        this.taskData = new TextBlock('', this.view.taskData);
        this.logEntries = new ListGroup(this.view.logEntries);
        new Command(this.previousTask.bind(this)).add(view.previousTaskButton);
        new Command(this.nextTask.bind(this)).add(view.nextTaskButton);
        new Command(this.back.bind(this)).add(view.backButton);
    }

    private back() {
        this.awaitable.resolve(
            TaskDetailPanelResult.backRequested
        );
    }

    private previousTask() {
        let currentIndex = this.tasks.indexOf(this.currentTask);
        let previousTask = this.tasks[currentIndex - 1];
        if (previousTask) {
            this.setCurrentTask(previousTask);
        }
        else {
            this.back();
        }
    }

    private nextTask() {
        let currentIndex = this.tasks.indexOf(this.currentTask);
        let nextTask = this.tasks[currentIndex + 1];
        if (nextTask) {
            this.setCurrentTask(nextTask);
        }
        else {
            this.back();
        }
    }

    setTasks(tasks: ITriggeredJobTaskModel[]) {
        this.tasks = tasks;
    }

    setCurrentTask(currentTask: ITriggeredJobTaskModel) {
        this.currentTask = currentTask;
        this.displayText.setText(currentTask.TaskDefinition.TaskKey.DisplayText);
        this.status.setText(currentTask.Status.DisplayText);
        this.timeStarted.setText(
            currentTask.TimeStarted.getFullYear() < 9999 ?
                new FormattedDate(currentTask.TimeStarted).formatDateTime() :
                ''
        );
        this.timeElapsed.setText(
            new FormattedTimeSpan(currentTask.TimeStarted, currentTask.TimeEnded).format()
        );
        this.taskData.setText(currentTask.TaskData);
        if (currentTask.TaskData) {
            this.view.taskData.show();
        }
        else {
            this.view.taskData.hide();
        }
        this.logEntries.setItems(
            currentTask.LogEntries,
            (entry, itemView: LogEntryItemView) => new LogEntryItem(entry, itemView)
        );
    }

    start() {
        return this.awaitable.start();
    }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }
}