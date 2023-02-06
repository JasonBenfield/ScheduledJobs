import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand, Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/Components/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/Components/MessageAlert";
import { ScheduledJobsAppApi } from "../../Lib/Api/ScheduledJobsAppApi";
import { JobListPanelView } from "../Jobs/JobListPanelView";
import { JobSummaryListItem } from "../Jobs/JobSummaryListItem";
import { JobSummaryListItemView } from "../Jobs/JobSummaryListItemView";

interface IResults {
    back?: boolean;
}

export class JobListPanelResult {
    static back() { return new JobListPanelResult({ back: true }); }

    private constructor(private readonly results: IResults) { }

    get back() { return this.results.back; }
}

export class JobListPanel implements IPanel {
    private readonly awaitable = new Awaitable<JobListPanelResult>();
    private readonly alert: MessageAlert;
    private readonly triggeredJobs: ListGroup<JobSummaryListItem, JobSummaryListItemView>;
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
        const jobs = await this.getRecentTriggeredJobs();
        this.triggeredJobs.setItems(
            jobs,
            (job, itemView) => new JobSummaryListItem(this.schdJobsApi, job, itemView)
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