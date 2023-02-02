import { HubAppApi } from "@jasonbenfield/hubwebapp/Api/HubAppApi";
import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand, Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/Components/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/Components/MessageAlert";
import { ModalConfirm } from "@jasonbenfield/sharedwebapp/Components/ModalConfirm";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { First } from "@jasonbenfield/sharedwebapp/Enumerable";
import { FormattedDate } from "@jasonbenfield/sharedwebapp/FormattedDate";
import { JobTaskStatus } from "../../../Lib/Api/JobTaskStatus";
import { ScheduledJobsAppApi } from "../../../Lib/Api/ScheduledJobsAppApi";
import { FormattedTimeSpan } from "../../FormattedTimeSpan";
import { LogEntryItem } from "./LogEntryItem";
import { LogEntryItemView } from "./LogEntryItemView";
import { TaskDetailPanelView } from "./TaskDetailPanelView";

interface IResult {
    backRequested?: { refreshRequired: boolean; };
    editTaskRequested?: { task: ITriggeredJobTaskModel; };
}

class Result {
    static backRequested(refreshRequired: boolean) {
        return new Result({ backRequested: { refreshRequired: refreshRequired } });
    }

    static editTaskRequested(task: ITriggeredJobTaskModel) {
        return new Result({ editTaskRequested: { task: task } });
    }

    private constructor(private readonly results: IResult) { }

    get backRequested() { return this.results.backRequested; }

    get editTaskRequested() { return this.results.editTaskRequested; }
}

export class TaskDetailPanel implements IPanel {
    private readonly awaitable = new Awaitable<Result>();
    private readonly displayText: TextComponent;
    private readonly status: TextComponent;
    private readonly timeStarted: TextComponent;
    private readonly timeElapsed: TextComponent;
    private readonly taskData: TextComponent;
    private readonly logEntries: ListGroup<LogEntryItem, LogEntryItemView>;
    private readonly alert: MessageAlert;
    private tasks: ITriggeredJobTaskModel[];
    private sourceLogEntries: ISourceLogEntryModel[];
    private currentTask: ITriggeredJobTaskModel;
    private readonly timeoutTaskCommand: AsyncCommand;
    private readonly editTaskDataCommand: Command;
    private readonly cancelTaskCommand: AsyncCommand;
    private readonly retryTaskCommand: AsyncCommand;
    private readonly skipTaskCommand: AsyncCommand;
    private readonly modalConfirm: ModalConfirm;

    constructor(private readonly hubApi: HubAppApi, private readonly schdJobsApi: ScheduledJobsAppApi, private view: TaskDetailPanelView) {
        this.displayText = new TextComponent(view.displayText);
        this.status = new TextComponent(view.status);
        this.timeStarted = new TextComponent(view.timeStarted);
        this.timeElapsed = new TextComponent(view.timeElapsed);
        this.taskData = new TextComponent(view.taskData);
        this.logEntries = new ListGroup(view.logEntries);
        this.alert = new MessageAlert(view.alert);
        new Command(this.previousTask.bind(this)).add(view.previousTaskButton);
        new Command(this.nextTask.bind(this)).add(view.nextTaskButton);
        new Command(this.back.bind(this)).add(view.backButton);
        this.timeoutTaskCommand = new AsyncCommand(this.timeoutTask.bind(this));
        this.timeoutTaskCommand.hide();
        this.timeoutTaskCommand.add(view.timeoutTaskButton);
        this.cancelTaskCommand = new AsyncCommand(this.cancelTask.bind(this));
        this.cancelTaskCommand.hide();
        this.cancelTaskCommand.add(view.cancelTaskButton);
        this.retryTaskCommand = new AsyncCommand(this.retryTask.bind(this));
        this.retryTaskCommand.hide();
        this.retryTaskCommand.add(view.retryTaskButton);
        this.skipTaskCommand = new AsyncCommand(this.skipTask.bind(this));
        this.skipTaskCommand.hide();
        this.skipTaskCommand.add(view.skipTaskButton);
        this.editTaskDataCommand = new Command(this.editTask.bind(this));
        this.editTaskDataCommand.add(view.editTaskDataButton);
        this.editTaskDataCommand.hide();
        this.modalConfirm = new ModalConfirm(view.modalConfirm);
    }

    private async timeoutTask() {
        const confirmed = await this.modalConfirm.confirm('Cause this task to timeout?', 'Confirm timeout');
        if (confirmed) {
            await this.alert.infoAction(
                'Timing out task...',
                () => this.schdJobsApi.Tasks.TimeoutTask({ TaskID: this.currentTask.ID })
            );
            this.awaitable.resolve(Result.backRequested(true));
        }
    }

    private async cancelTask() {
        const confirmed = await this.modalConfirm.confirm('Cancel this task?', 'Confirm cancel');
        if (confirmed) {
            await this.alert.infoAction(
                'Canceling task...',
                () => this.schdJobsApi.Tasks.CancelTask({ TaskID: this.currentTask.ID })
            );
            this.awaitable.resolve(Result.backRequested(true));
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
                Result.backRequested(true)
            );
        }
    }

    private async skipTask() {
        const confirmed = await this.modalConfirm.confirm('Skip this task?', 'Confirm skip');
        if (confirmed) {
            await this.alert.infoAction(
                'Skipping task...',
                () => this.schdJobsApi.Tasks.SkipTask({ TaskID: this.currentTask.ID })
            );
            this.awaitable.resolve(
                Result.backRequested(true)
            );
        }
    }

    private editTask() {
        this.awaitable.resolve(Result.editTaskRequested(this.currentTask));
    }

    private back() {
        this.awaitable.resolve(
            Result.backRequested(false)
        );
    }

    private previousTask() {
        const currentIndex = this.tasks.indexOf(this.currentTask);
        const previousTask = this.tasks[currentIndex - 1];
        if (previousTask) {
            this.setCurrentTask(previousTask);
        }
        else {
            this.back();
        }
    }

    private nextTask() {
        const currentIndex = this.tasks.indexOf(this.currentTask);
        const nextTask = this.tasks[currentIndex + 1];
        if (nextTask) {
            this.setCurrentTask(nextTask);
        }
        else {
            this.back();
        }
    }

    setTasks(tasks: ITriggeredJobTaskModel[], sourceLogEntries: ISourceLogEntryModel[]) {
        this.tasks = tasks;
        this.sourceLogEntries = sourceLogEntries;
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
            (entry, itemView) => {
                const sourceLogEntry = this.sourceLogEntries.find(le => le.LogEntryID === entry.ID);
                return new LogEntryItem(this.hubApi, entry, sourceLogEntry, itemView);
            }
        );
        const status = JobTaskStatus.values.value(currentTask.Status.Value);
        if (status.equals(JobTaskStatus.values.Failed)) {
            this.cancelTaskCommand.show();
            this.retryTaskCommand.show();
            this.skipTaskCommand.show();
            this.editTaskDataCommand.show();
        }
        else {
            this.cancelTaskCommand.hide();
            this.retryTaskCommand.hide();
            this.skipTaskCommand.hide();
            this.editTaskDataCommand.hide();
        }
        if (status.equals(JobTaskStatus.values.Running)) {
            this.timeoutTaskCommand.show();
        }
        else {
            this.timeoutTaskCommand.hide();
        }
    }

    start() {
        return this.awaitable.start();
    }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }
}