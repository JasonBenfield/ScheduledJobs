import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand } from "@jasonbenfield/sharedwebapp/Command/AsyncCommand";
import { Command } from "@jasonbenfield/sharedwebapp/Command/Command";
import { TextBlock } from "@jasonbenfield/sharedwebapp/Html/TextBlock";
import { ListGroup } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/MessageAlert";
import { ScheduledJobsAppApi } from "../../../ScheduledJobs/Api/ScheduledJobsAppApi";
import { JobListPanelView } from "../JobListPanelView";
import { JobSummaryListItem } from "../JobSummaryListItem";
import { JobSummaryListItemView } from "../JobSummaryListItemView";

interface IResults {
    menuRequested?: {};
}

export class RecentJobsPanelResult {
    static menuRequested() { return new RecentJobsPanelResult({ menuRequested: {} }); }

    private constructor(private readonly results: IResults) { }

    get menuRequested() { return this.results.menuRequested; }
}

export class RecentJobsPanel implements IPanel {
    private readonly awaitable = new Awaitable<RecentJobsPanelResult>();
    private readonly alert: MessageAlert;
    private readonly failedJobsList: ListGroup;
    private readonly refreshCommand: AsyncCommand;

    constructor(private readonly schdJobsApi: ScheduledJobsAppApi, private readonly view: JobListPanelView) {
        this.alert = new MessageAlert(view.alert);
        this.failedJobsList = new ListGroup(view.jobs);
        new TextBlock('Recent Jobs', view.heading);
        new Command(this.requestMenu.bind(this)).add(view.menuButton);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.add(view.refreshButton);
        this.refreshCommand.animateIconWhenInProgress('spin');
    }

    private requestMenu() { return RecentJobsPanelResult.menuRequested(); }

    private async doRefresh() {
        let failedJobs = await this.getRecentJobs();
        this.failedJobsList.setItems(
            failedJobs,
            (job, itemView: JobSummaryListItemView) => new JobSummaryListItem(this.schdJobsApi, job, itemView)
        );
        if (failedJobs.length === 0) {
            this.alert.danger('No jobs were found.');
        }
    }

    private async getRecentJobs() {
        let recentJobs: IJobSummaryModel[];
        await this.alert.infoAction(
            'Loading...',
            async () => {
                recentJobs = await this.schdJobsApi.JobInquiry.GetRecentJobs();
            }
        );
        return recentJobs;
    }

    refresh() { return this.refreshCommand.execute(); }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }
}