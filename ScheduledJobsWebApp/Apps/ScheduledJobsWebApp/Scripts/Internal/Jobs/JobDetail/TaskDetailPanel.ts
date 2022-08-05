import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand, Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/Components/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/Components/MessageAlert";
import { ModalConfirm } from "@jasonbenfield/sharedwebapp/Components/ModalConfirm";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { FormattedDate } from "@jasonbenfield/sharedwebapp/FormattedDate";
import { JobTaskStatus } from "../../../Lib/Api/JobTaskStatus";
import { ScheduledJobsAppApi } from "../../../Lib/Api/ScheduledJobsAppApi";
import { FormattedTimeSpan } from "../../FormattedTimeSpan";
import { LogEntryItem } from "./LogEntryItem";
import { LogEntryItemView } from "./LogEntryItemView";
import { TaskDetailPanelView } from "./TaskDetailPanelView";

interface Results {
    backRequested?: { refreshRequired: boolean; };
}

export class TaskDetailPanelResult {
    static backRequested(refreshRequired: boolean) {
        return new TaskDetailPanelResult({ backRequested: { refreshRequired: refreshRequired } });
    }

    private constructor(private readonly results: Results) { }

    get backRequested() { return this.results.backRequested; }
}

export class TaskDetailPanel implements IPanel {
    private readonly awaitable = new Awaitable<TaskDetailPanelResult>();
    private readonly displayText: TextComponent;
    private readonly status: TextComponent;
    private readonly timeStarted: TextComponent;
    private readonly timeElapsed: TextComponent;
    private readonly taskData: TextComponent;
    private readonly logEntries: ListGroup;
    private readonly alert: MessageAlert;
    private tasks: ITriggeredJobTaskModel[];
    private currentTask: ITriggeredJobTaskModel;
    private readonly cancelTaskCommand: AsyncCommand;
    private readonly retryTaskCommand: AsyncCommand;
    private readonly modalConfirm: ModalConfirm;

    constructor(private readonly schdJobsApi: ScheduledJobsAppApi, private view: TaskDetailPanelView) {
        this.displayText = new TextComponent(this.view.displayText);
        this.status = new TextComponent(this.view.status);
        this.timeStarted = new TextComponent(this.view.timeStarted);
        this.timeElapsed = new TextComponent(this.view.timeElapsed);
        this.taskData = new TextComponent(this.view.taskData);
        this.logEntries = new ListGroup(this.view.logEntries);
        this.alert = new MessageAlert(this.view.alert);
        new Command(this.previousTask.bind(this)).add(view.previousTaskButton);
        new Command(this.nextTask.bind(this)).add(view.nextTaskButton);
        new Command(this.back.bind(this)).add(view.backButton);
        this.cancelTaskCommand = new AsyncCommand(this.cancelTask.bind(this));
        this.cancelTaskCommand.hide();
        this.cancelTaskCommand.add(view.cancelTaskButton);
        this.retryTaskCommand = new AsyncCommand(this.retryTask.bind(this));
        this.retryTaskCommand.hide();
        this.retryTaskCommand.add(view.retryTaskButton);
        this.modalConfirm = new ModalConfirm(view.modalConfirm);
    }

    private async cancelTask() {
        const confirmed = await this.modalConfirm.confirm('Cancel this task?', 'Confirm cancel');
        if (confirmed) {
            await this.alert.infoAction(
                'Canceling task...',
                () => this.schdJobsApi.Tasks.CancelTask({ TaskID: this.currentTask.ID })
            );
            this.awaitable.resolve(
                TaskDetailPanelResult.backRequested(true)
            );
        }
    }

    private async retryTask() {
        const confirmed = await this.modalConfirm.confirm('Retry this task?', 'Confirm retry');
        if (confirmed) {
            await this.alert.infoAction(
                'Retrying task...',
                () => this.schdJobsApi.Tasks.RetryTask({ TaskID: this.currentTask.ID })
            );
            this.awaitable.resolve(
                TaskDetailPanelResult.backRequested(true)
            );
        }
    }

    private back() {
        this.awaitable.resolve(
            TaskDetailPanelResult.backRequested(false)
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
        let status = JobTaskStatus.values.value(currentTask.Status.Value);
        if (status.equals(JobTaskStatus.values.Failed)) {
            this.cancelTaskCommand.show();
            this.retryTaskCommand.show();
        }
        else {
            this.cancelTaskCommand.hide();
            this.retryTaskCommand.hide();
        }
    }

    start() {
        return this.awaitable.start();
    }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }
}