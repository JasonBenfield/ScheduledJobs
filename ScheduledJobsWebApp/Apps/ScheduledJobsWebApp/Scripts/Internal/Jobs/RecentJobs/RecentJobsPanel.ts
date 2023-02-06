import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand, Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/Components/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/Components/MessageAlert";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { ScheduledJobsAppApi } from "../../../Lib/Api/ScheduledJobsAppApi";
import { JobListPanelView } from "../JobListPanelView";
import { JobSummaryListItem } from "../JobSummaryListItem";
import { JobSummaryListItemView } from "../JobSummaryListItemView";

interface IResults {
    menuRequested?: boolean;
}

export class RecentJobsPanelResult {
    static menuRequested() { return new RecentJobsPanelResult({ menuRequested: true }); }

    private constructor(private readonly results: IResults) { }

    get menuRequested() { return this.results.menuRequested; }
}

export class RecentJobsPanel implements IPanel {
    private readonly awaitable = new Awaitable<RecentJobsPanelResult>();
    private readonly alert: MessageAlert;
    private readonly recentJobsList: ListGroup<JobSummaryListItem, JobSummaryListItemView>;
    private readonly refreshCommand: AsyncCommand;

    constructor(private readonly schdJobsApi: ScheduledJobsAppApi, private readonly view: JobListPanelView) {
        this.alert = new MessageAlert(view.alert);
        this.recentJobsList = new ListGroup(view.jobs);
        new TextComponent(view.heading).setText('Recent Jobs');
        new Command(this.requestMenu.bind(this)).add(view.menuButton);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.add(view.refreshButton);
        this.refreshCommand.animateIconWhenInProgress('spin');
    }

    private requestMenu() { this.awaitable.resolve(RecentJobsPanelResult.menuRequested()); }

    private async doRefresh() {
        const recentJobs = await this.getRecentJobs();
        this.recentJobsList.setItems(
            recentJobs,
            (job, itemView) => new JobSummaryListItem(this.schdJobsApi, job, itemView)
        );
        if (recentJobs.length === 0) {
            this.alert.danger('No jobs were found.');
        }
    }

    private getRecentJobs() {
        return this.alert.infoAction(
            'Loading...',
            () => this.schdJobsApi.JobInquiry.GetRecentJobs()
        );
    }

    refresh() { return this.refreshCommand.execute(); }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }
}