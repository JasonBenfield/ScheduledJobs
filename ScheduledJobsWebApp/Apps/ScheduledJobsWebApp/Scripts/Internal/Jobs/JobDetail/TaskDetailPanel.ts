import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand } from "@jasonbenfield/sharedwebapp/Command/AsyncCommand";
import { Command } from "@jasonbenfield/sharedwebapp/Command/Command";
import { FormattedDate } from "@jasonbenfield/sharedwebapp/FormattedDate";
import { TextBlock } from "@jasonbenfield/sharedwebapp/Html/TextBlock";
import { ListGroup } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/MessageAlert";
import { ModalConfirmComponent } from "@jasonbenfield/sharedwebapp/Modal/ModalConfirmComponent";
import { JobTaskStatus } from "../../../ScheduledJobs/Api/JobTaskStatus";
import { ScheduledJobsAppApi } from "../../../ScheduledJobs/Api/ScheduledJobsAppApi";
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
    private readonly displayText: TextBlock;
    private readonly status: TextBlock;
    private readonly timeStarted: TextBlock;
    private readonly timeElapsed: TextBlock;
    private readonly taskData: TextBlock;
    private readonly logEntries: ListGroup;
    private readonly alert: MessageAlert;
    private tasks: ITriggeredJobTaskModel[];
    private currentTask: ITriggeredJobTaskModel;
    private readonly cancelTaskCommand: AsyncCommand;
    private readonly retryTaskCommand: AsyncCommand;
    private readonly modalConfirm: ModalConfirmComponent;

    constructor(private readonly schdJobsApi: ScheduledJobsAppApi, private view: TaskDetailPanelView) {
        this.displayText = new TextBlock('', this.view.displayText);
        this.status = new TextBlock('', this.view.status);
        this.timeStarted = new TextBlock('', this.view.timeStarted);
        this.timeElapsed = new TextBlock('', this.view.timeElapsed);
        this.taskData = new TextBlock('', this.view.taskData);
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
        this.modalConfirm = new ModalConfirmComponent(view.modalConfirm);
    }

    private async cancelTask() {
        let confirmed = await this.modalConfirm.confirm('Cancel this task?', 'Confirm cancel');
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
        let confirmed = await this.modalConfirm.confirm('Retry this task?', 'Confirm retry');
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