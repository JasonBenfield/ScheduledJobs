﻿import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { CardAlert } from "@jasonbenfield/sharedwebapp/Components/CardAlert";
import { AsyncCommand, Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/Components/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/Components/MessageAlert";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { ScheduledJobsAppClient } from "../../../Lib/Http/ScheduledJobsAppClient";
import { JobListPanelView } from "../JobListPanelView";
import { JobSummaryListItem } from "../JobSummaryListItem";
import { JobSummaryListItemView } from "../JobSummaryListItemView";
import { IMessageAlert } from "@jasonbenfield/sharedwebapp/Components/Types";

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
    private readonly alert: IMessageAlert;
    private readonly countTextComponent: TextComponent;
    private readonly recentJobsList: ListGroup<JobSummaryListItem, JobSummaryListItemView>;
    private readonly refreshCommand: AsyncCommand;

    constructor(private readonly schdJobsClient: ScheduledJobsAppClient, private readonly view: JobListPanelView) {
        this.alert = new CardAlert(view.alert);
        this.recentJobsList = new ListGroup(view.jobListView);
        new TextComponent(view.titleTextView).setText('Recent Jobs');
        this.countTextComponent = new TextComponent(view.countTextView);
        this.countTextComponent.hide();
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
            (job, itemView) => new JobSummaryListItem(this.schdJobsClient, job, itemView)
        );
        if (recentJobs.length === 0) {
            this.alert.danger('No jobs were found.');
        }
    }

    private getRecentJobs() {
        return this.alert.infoAction(
            'Loading...',
            () => this.schdJobsClient.JobInquiry.GetRecentJobs()
        );
    }

    refresh() { return this.refreshCommand.execute(); }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }
}