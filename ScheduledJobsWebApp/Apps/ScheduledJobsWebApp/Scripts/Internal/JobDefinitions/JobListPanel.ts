import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand } from "@jasonbenfield/sharedwebapp/Command/AsyncCommand";
import { Command } from "@jasonbenfield/sharedwebapp/Command/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/MessageAlert";
import { ScheduledJobsAppApi } from "../../ScheduledJobs/Api/ScheduledJobsAppApi";
import { JobListPanelView } from "../Jobs/JobListPanelView";
import { JobSummaryListItem } from "../Jobs/JobSummaryListItem";
import { JobSummaryListItemView } from "../Jobs/JobSummaryListItemView";

interface IResults {
    back?: {};
}

export class JobListPanelResult {
    static back() { return new JobListPanelResult({ back: {} }); }

    private constructor(private readonly results: IResults) { }

    get back() { return this.results.back; }
}

export class JobListPanel implements IPanel {
    private readonly awaitable = new Awaitable<JobListPanelResult>();
    private readonly alert: MessageAlert;
    private readonly triggeredJobs: ListGroup;
    private readonly refreshCommand: AsyncCommand;
    private jobDefinitionID: number;

    constructor(private readonly schdJobsApi: ScheduledJobsAppApi, private readonly view: JobListPanelView) {
        view.menuButton.hide();
        view.backButton.show();
        this.alert = new MessageAlert(view.alert);
        this.triggeredJobs = new ListGroup(view.jobs);
        new Command(this.back.bind(this)).add(view.backButton);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.add(view.refreshButton);
        this.refreshCommand.animateIconWhenInProgress('spin');
    }

    private back() { this.awaitable.resolve(JobListPanelResult.back()); }

    private async doRefresh() {
        let jobs = await this.getRecentTriggeredJobs();
        this.triggeredJobs.setItems(
            jobs,
            (job, itemView: JobSummaryListItemView) => new JobSummaryListItem(this.schdJobsApi, job, itemView)
        );
    }

    private getRecentTriggeredJobs() {
        return this.alert.infoAction(
            'Loading...',
            () => this.schdJobsApi.JobDefinitions.GetRecentTriggeredJobs({
                JobDefinitionID: this.jobDefinitionID
            })
        );
    }

    setJobDefinitionID(jobDefinitionID: number) {
        this.jobDefinitionID = jobDefinitionID;
    }

    refresh() { return this.refreshCommand.execute(); }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }
}