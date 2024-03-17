import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand, Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/Components/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/Components/MessageAlert";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { TextLinkComponent } from "@jasonbenfield/sharedwebapp/Components/TextLinkComponent";
import { ScheduledJobsAppClient } from "../../../Lib/Http/ScheduledJobsAppClient";
import { JobDetailPanelView } from "./JobDetailPanelView";
import { TaskListItem } from "./TaskListItem";
import { TaskListItemView } from "./TaskListItemView";

interface IResult {
    menuRequested?: boolean;
    taskSelected?: {
        tasks: ITriggeredJobTaskModel[],
        sourceLogEntries: ISourceLogEntryModel[],
        selectedTask: ITriggeredJobTaskModel
    };
}

class Result {
    static menuRequested() { return new Result({ menuRequested: true }); }

    static taskSelected(tasks: ITriggeredJobTaskModel[], sourceLogEntries: ISourceLogEntryModel[], selectedTask: ITriggeredJobTaskModel) {
        return new Result({ taskSelected: { tasks: tasks, sourceLogEntries: sourceLogEntries, selectedTask: selectedTask } });
    }

    private constructor(private readonly results: IResult) { }

    get menuRequested() { return this.results.menuRequested; }

    get taskSelected() { return this.results.taskSelected; }
}

export class JobDetailPanel implements IPanel {
    private readonly awaitable = new Awaitable<Result>();
    private readonly alert: MessageAlert;
    private readonly jobDisplayText: TextComponent;
    private readonly triggeredByLink: TextLinkComponent;
    private readonly taskList: ListGroup<TaskListItem, TaskListItemView>;
    private readonly refreshCommand: AsyncCommand;
    private jobID: number;
    private jobDetail: ITriggeredJobDetailModel;

    constructor(private readonly schdJobsClient: ScheduledJobsAppClient, private readonly view: JobDetailPanelView) {
        this.view.hideJob();
        this.alert = new MessageAlert(this.view.alert);
        this.jobDisplayText = new TextComponent(this.view.jobDisplayText);
        this.triggeredByLink = new TextLinkComponent(this.view.triggeredByLink);
        this.taskList = new ListGroup(this.view.tasks);
        this.taskList.when.itemClicked.then(this.onTaskClicked.bind(this));
        new Command(this.requestMenu.bind(this)).add(view.menuButton);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.add(view.refreshButton);
        this.refreshCommand.animateIconWhenInProgress('spin');
    }

    private requestMenu() { this.awaitable.resolve(Result.menuRequested()); }

    private async doRefresh() {
        this.jobDetail = await this.getJobDetail(this.jobID);
        this.jobDisplayText.setText(this.jobDetail.Job.JobDefinition.JobKey.DisplayText);
        this.triggeredByLink.setText(this.jobDetail.TriggeredBy.Definition.EventKey.DisplayText);
        this.triggeredByLink.setHref(
            this.schdJobsClient.EventInquiry.NotificationDetail.getUrl({
                NotificationID: this.jobDetail.TriggeredBy.ID
            }).value()
        );
        this.taskList.setItems(
            this.jobDetail.Tasks,
            (task, itemView) => new TaskListItem(task, itemView)
        );
        if (this.jobDetail.Tasks.length === 0) {
            this.alert.danger('No Tasks have been started for this  job.');
        }
        this.view.showJob();
    }

    private getJobDetail(jobID) {
        return this.alert.infoAction(
            'Loading...',
            () => this.schdJobsClient.JobInquiry.GetJobDetail({ JobID: jobID })
        );
    }

    private onTaskClicked(taskItem: TaskListItem) {
        this.awaitable.resolve(
            Result.taskSelected(this.jobDetail.Tasks, this.jobDetail.SourceLogEntries, taskItem.task)
        );
    }

    setJobID(jobID: number) {
        this.jobID = jobID;
    }

    refresh() { return this.refreshCommand.execute(); }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }

}