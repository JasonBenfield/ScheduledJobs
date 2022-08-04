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
    menuRequested?: {};
}

export class FailedJobsPanelResult {
    static menuRequested() { return new FailedJobsPanelResult({ menuRequested: {} }); }

    private constructor(private readonly results: IResults) { }

    get menuRequested() { return this.results.menuRequested; }
}

export class FailedJobsPanel implements IPanel {
    private readonly awaitable = new Awaitable<FailedJobsPanelResult>();
    private readonly alert: MessageAlert;
    private readonly failedJobsList: ListGroup;
    private readonly refreshCommand: AsyncCommand;

    constructor(private readonly schdJobsApi: ScheduledJobsAppApi, private readonly view: JobListPanelView) {
        this.alert = new MessageAlert(view.alert);
        this.failedJobsList = new ListGroup(view.jobs);
        new TextComponent(view.heading).setText('Failed Jobs');
        new Command(this.requestMenu.bind(this)).add(view.menuButton);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.add(view.refreshButton);
        this.refreshCommand.animateIconWhenInProgress('spin');
    }

    private requestMenu() { return FailedJobsPanelResult.menuRequested(); }

    private async doRefresh() {
        const failedJobs = await this.getFailedJobs();
        this.failedJobsList.setItems(
            failedJobs,
            (job, itemView: JobSummaryListItemView) => new JobSummaryListItem(this.schdJobsApi, job, itemView)
        );
        if (failedJobs.length === 0) {
            this.alert.success('No failed jobs were found.');
        }
    }

    private async getFailedJobs() {
        let failedJobs: IJobSummaryModel[];
        await this.alert.infoAction(
            'Loading...',
            async () => {
                failedJobs = await this.schdJobsApi.JobInquiry.GetFailedJobs();
            }
        );
        return failedJobs;
    }

    refresh() { return this.refreshCommand.execute(); }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }
}