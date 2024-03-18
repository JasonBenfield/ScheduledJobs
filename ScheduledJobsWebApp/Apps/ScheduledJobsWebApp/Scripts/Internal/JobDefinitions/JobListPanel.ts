import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand, Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/Components/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/Components/MessageAlert";
import { ScheduledJobsAppClient } from "../../Lib/Http/ScheduledJobsAppClient";
import { JobListPanelView } from "../Jobs/JobListPanelView";
import { JobSummaryListItem } from "../Jobs/JobSummaryListItem";
import { JobSummaryListItemView } from "../Jobs/JobSummaryListItemView";
import { CardAlert } from "@jasonbenfield/sharedwebapp/Components/CardAlert";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";

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
    private readonly titleTextComponet: TextComponent;
    private readonly countTextComponent: TextComponent;
    private readonly alert: MessageAlert;
    private readonly triggeredJobs: ListGroup<JobSummaryListItem, JobSummaryListItemView>;
    private readonly refreshCommand: AsyncCommand;
    private jobDefinitionID: number;

    constructor(private readonly schdJobsClient: ScheduledJobsAppClient, private readonly view: JobListPanelView) {
        view.menuButton.hide();
        view.backButton.show();
        this.titleTextComponet = new TextComponent(view.titleTextView);
        this.titleTextComponet.setText('Recent Jobs');
        this.countTextComponent = new TextComponent(view.countTextView);
        this.alert = new CardAlert(view.alert).alert;
        this.triggeredJobs = new ListGroup(view.jobs);
        new Command(this.back.bind(this)).add(view.backButton);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.add(view.refreshButton);
        this.refreshCommand.animateIconWhenInProgress('spin');
    }

    private back() { this.awaitable.resolve(JobListPanelResult.back()); }

    private async doRefresh() {
        this.countTextComponent.hide();
        const jobs = await this.getRecentTriggeredJobs();
        this.triggeredJobs.setItems(
            jobs,
            (job, itemView) => new JobSummaryListItem(this.schdJobsClient, job, itemView)
        );
        if (jobs.length === 0) {
            this.alert.danger('No Recent Jobs were found.');
        }
        else if (jobs.length > 1) {
            this.countTextComponent.setText(jobs.length.toLocaleString());
            this.countTextComponent.show();
        }
    }

    private getRecentTriggeredJobs() {
        return this.alert.infoAction(
            'Loading...',
            () => this.schdJobsClient.JobDefinitions.GetRecentTriggeredJobs({
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