import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand, Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/Components/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/Components/MessageAlert";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { ScheduledJobsAppClient } from "../../../Lib/Http/ScheduledJobsAppClient";
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
    private readonly alert: MessageAlert;
    private readonly failedJobsList: ListGroup<JobSummaryListItem, JobSummaryListItemView>;
    private readonly refreshCommand: AsyncCommand;

    constructor(private readonly schdJobsClient: ScheduledJobsAppClient, private readonly view: JobListPanelView) {
        this.alert = new MessageAlert(view.alert);
        this.failedJobsList = new ListGroup(view.jobs);
        new TextComponent(view.heading).setText('Failed Jobs');
        new Command(this.requestMenu.bind(this)).add(view.menuButton);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.add(view.refreshButton);
        this.refreshCommand.animateIconWhenInProgress('spin');
    }

    private requestMenu() { this.awaitable.resolve(FailedJobsPanelResult.menuRequested()); }

    private async doRefresh() {
        const failedJobs = await this.getFailedJobs();
        this.failedJobsList.setItems(
            failedJobs,
            (job, itemView) => new JobSummaryListItem(this.schdJobsClient, job, itemView)
        );
        if (failedJobs.length === 0) {
            this.alert.success('No failed jobs were found.');
        }
    }

    private getFailedJobs() {
        return this.alert.infoAction(
            'Loading...',
            () => this.schdJobsClient.JobInquiry.GetFailedJobs()
        );
    }

    refresh() { return this.refreshCommand.execute(); }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }
}