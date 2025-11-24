import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { CardAlert } from "@jasonbenfield/sharedwebapp/Components/CardAlert";
import { AsyncCommand, Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/Components/ListGroup";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { IMessageAlert } from "@jasonbenfield/sharedwebapp/Components/Types";
import { ScheduledJobsAppClient } from "../../../Lib/Http/ScheduledJobsAppClient";
import { JobSummary } from "../../../Lib/JobSummary";
import { JobListPanelView } from "../JobListPanelView";
import { JobSummaryListItem } from "../JobSummaryListItem";
import { JobSummaryListItemView } from "../JobSummaryListItemView";

interface IResults {
    menuRequested?: boolean;
}

export class FailedJobsPanelResult {
    static menuRequested() { return new FailedJobsPanelResult({ menuRequested: true }); }

    private constructor(private readonly results: IResults) { }

    get menuRequested() { return this.results.menuRequested; }
}

export class FailedJobsPanel implements IPanel {
    private readonly awaitable = new Awaitable<FailedJobsPanelResult>();
    private readonly alert: IMessageAlert;
    private readonly failedJobsList: ListGroup<JobSummaryListItem, JobSummaryListItemView>;
    private readonly refreshCommand: AsyncCommand;
    private readonly countTextComponent: TextComponent;

    constructor(private readonly schdJobsClient: ScheduledJobsAppClient, private readonly view: JobListPanelView) {
        this.alert = new CardAlert(view.alert);
        this.failedJobsList = new ListGroup(view.jobListView);
        new TextComponent(view.titleTextView).setText("Failed Jobs");
        this.countTextComponent = new TextComponent(view.countTextView);
        new Command(this.requestMenu.bind(this)).add(view.menuButton);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.add(view.refreshButton);
        this.refreshCommand.animateIconWhenInProgress("spin");
    }

    private requestMenu() { this.awaitable.resolve(FailedJobsPanelResult.menuRequested()); }

    private async doRefresh() {
        this.countTextComponent.hide();
        const sourceFailedJobs = await this.getFailedJobs();
        const failedJobs = sourceFailedJobs.map(j => new JobSummary(j));
        this.failedJobsList.setItems(
            failedJobs,
            (job, itemView) => new JobSummaryListItem(this.schdJobsClient, job, itemView)
        );
        if (failedJobs.length === 0) {
            this.alert.success("No failed jobs were found.");
        }
        else if (failedJobs.length > 1) {
            this.countTextComponent.setText(failedJobs.length.toLocaleString());
            this.countTextComponent.show();
        }
    }

    private getFailedJobs() {
        return this.alert.infoAction(
            "Loading...",
            () => this.schdJobsClient.JobInquiry.GetFailedJobs()
        );
    }

    refresh() { return this.refreshCommand.execute(); }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }
}