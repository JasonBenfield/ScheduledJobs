import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand } from "@jasonbenfield/sharedwebapp/Command/AsyncCommand";
import { Command } from "@jasonbenfield/sharedwebapp/Command/Command";
import { FormGroup } from "@jasonbenfield/sharedwebapp/Html/FormGroup";
import { TextBlock } from "@jasonbenfield/sharedwebapp/Html/TextBlock";
import { TextLink } from "@jasonbenfield/sharedwebapp/Html/TextLink";
import { ListGroup } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/MessageAlert";
import { ScheduledJobsAppApi } from "../../../ScheduledJobs/Api/ScheduledJobsAppApi";
import { JobDetailPanelView } from "./JobDetailPanelView";
import { TaskListItem } from "./TaskListItem";
import { TaskListItemView } from "./TaskListItemView";

interface Results {
    menuRequested?: {};
    taskSelected?: { tasks: ITriggeredJobTaskModel[], selectedTask: ITriggeredJobTaskModel };
}

export class JobDetailPanelResult {
    static menuRequested() { return new JobDetailPanelResult({ menuRequested: {} }); }

    static taskSelected(tasks: ITriggeredJobTaskModel[], selectedTask: ITriggeredJobTaskModel) {
        return new JobDetailPanelResult({ taskSelected: { tasks: tasks, selectedTask: selectedTask } });
    }

    private constructor(private readonly results: Results) { }

    get menuRequested() { return this.results.menuRequested; }

    get taskSelected() { return this.results.taskSelected; }
}

export class JobDetailPanel implements IPanel {
    private readonly awaitable = new Awaitable<JobDetailPanelResult>();
    private readonly alert: MessageAlert;
    private readonly jobDisplayText: TextBlock;
    private readonly triggeredByLink: TextLink;
    private readonly taskList: ListGroup;
    private readonly refreshCommand: AsyncCommand;
    private jobID: number;
    private jobDetail: ITriggeredJobDetailModel;

    constructor(private readonly schdJobsApi: ScheduledJobsAppApi, private readonly view: JobDetailPanelView) {
        this.view.hideJob();
        this.alert = new MessageAlert(this.view.alert);
        this.jobDisplayText = new TextBlock('', this.view.jobDisplayText);
        new FormGroup(this.view.triggeredByFormGroup).setCaption('Triggered By');
        this.triggeredByLink = new TextLink('', this.view.triggeredByLink);
        this.taskList = new ListGroup(this.view.tasks);
        this.taskList.itemClicked.register(this.onTaskClicked.bind(this));
        new Command(this.requestMenu.bind(this)).add(view.menuButton);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.add(view.refreshButton);
        this.refreshCommand.animateIconWhenInProgress('spin');
    }

    private requestMenu() { this.awaitable.resolve(JobDetailPanelResult.menuRequested()); }

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
            JobDetailPanelResult.taskSelected(this.jobDetail.Tasks, taskItem.task)
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