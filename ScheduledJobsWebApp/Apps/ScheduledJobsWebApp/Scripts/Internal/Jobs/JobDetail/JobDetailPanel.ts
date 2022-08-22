import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand, Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/Components/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/Components/MessageAlert";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { TextLinkComponent } from "@jasonbenfield/sharedwebapp/Components/TextLinkComponent";
import { ScheduledJobsAppApi } from "../../../Lib/Api/ScheduledJobsAppApi";
import { JobDetailPanelView } from "./JobDetailPanelView";
import { TaskListItem } from "./TaskListItem";
import { TaskListItemView } from "./TaskListItemView";

interface IResult {
    menuRequested?: {};
    taskSelected?: { tasks: ITriggeredJobTaskModel[], selectedTask: ITriggeredJobTaskModel };
}

class Result {
    static menuRequested() { return new Result({ menuRequested: {} }); }

    static taskSelected(tasks: ITriggeredJobTaskModel[], selectedTask: ITriggeredJobTaskModel) {
        return new Result({ taskSelected: { tasks: tasks, selectedTask: selectedTask } });
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
    private readonly taskList: ListGroup;
    private readonly refreshCommand: AsyncCommand;
    private jobID: number;
    private jobDetail: ITriggeredJobDetailModel;

    constructor(private readonly schdJobsApi: ScheduledJobsAppApi, private readonly view: JobDetailPanelView) {
        this.view.hideJob();
        this.alert = new MessageAlert(this.view.alert);
        this.jobDisplayText = new TextComponent(this.view.jobDisplayText);
        this.triggeredByLink = new TextLinkComponent(this.view.triggeredByLink);
        this.taskList = new ListGroup(this.view.tasks);
        this.taskList.registerItemClicked(this.onTaskClicked.bind(this));
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
            this.schdJobsApi.EventInquiry.NotificationDetail.getUrl({
                NotificationID: this.jobDetail.TriggeredBy.ID
            }).value()
        );
        this.taskList.setItems(
            this.jobDetail.Tasks,
            (task, itemView: TaskListItemView) => new TaskListItem(task, itemView)
        );
        this.view.showJob();
    }

    private async getJobDetail(jobID) {
        let jobDetail: ITriggeredJobDetailModel;
        await this.alert.infoAction(
            'Loading...',
            async () => {
                jobDetail = await this.schdJobsApi.JobInquiry.GetJobDetail({ JobID: jobID });
            }
        );
        return jobDetail;
    }

    private onTaskClicked(taskItem: TaskListItem) {
        this.awaitable.resolve(
            Result.taskSelected(this.jobDetail.Tasks, taskItem.task)
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