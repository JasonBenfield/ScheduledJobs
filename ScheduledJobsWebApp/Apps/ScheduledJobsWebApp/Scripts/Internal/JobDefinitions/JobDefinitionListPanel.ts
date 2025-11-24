import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { CardAlert } from "@jasonbenfield/sharedwebapp/Components/CardAlert";
import { AsyncCommand, Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/Components/ListGroup";
import { IMessageAlert } from "@jasonbenfield/sharedwebapp/Components/Types";
import { ScheduledJobsAppClient } from "../../Lib/Http/ScheduledJobsAppClient";
import { JobDefinition } from "../../Lib/JobDefinition";
import { JobDefinitionListItem } from "./JobDefinitionListItem";
import { JobDefinitionListItemView } from "./JobDefinitionListItemView";
import { JobDefinitionListPanelView } from "./JobDefinitionListPanelView";

interface IResults {
    menuRequested?: boolean;
    jobDefinitionSelected?: { jobDefinitionID: number; }
}

export class JobDefinitionListPanelResult {
    static menuRequested() { return new JobDefinitionListPanelResult({ menuRequested: true }); }

    static jobDefinitionSelected(jobDefinitionID: number) {
        return new JobDefinitionListPanelResult({
            jobDefinitionSelected: { jobDefinitionID: jobDefinitionID }
        });
    }

    private constructor(private readonly results: IResults) { }

    get menuRequested() { return this.results.menuRequested; }

    get jobDefinitionSelected() { return this.results.jobDefinitionSelected; }
}

export class JobDefinitionListPanel implements IPanel {
    private readonly awaitable = new Awaitable<JobDefinitionListPanelResult>();
    private readonly alert: IMessageAlert;
    private readonly jobDefinitions: ListGroup<JobDefinitionListItem, JobDefinitionListItemView>;
    private readonly refreshCommand: AsyncCommand;

    constructor(private readonly schdJobsClient: ScheduledJobsAppClient, private readonly view: JobDefinitionListPanelView) {
        this.alert = new CardAlert(view.alert);
        this.jobDefinitions = new ListGroup(view.jobDefinitionListView);
        this.jobDefinitions.when.itemClicked.then(this.onJobClicked.bind(this));
        new Command(this.requestMenu.bind(this)).add(view.menuButton);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.add(view.refreshButton);
        this.refreshCommand.animateIconWhenInProgress("spin");
    }

    private requestMenu() { this.awaitable.resolve(JobDefinitionListPanelResult.menuRequested()); }

    private async doRefresh() {
        const sourceJobDefs = await this.getJobDefinitions();
        const jobDefs = sourceJobDefs.map(d => new JobDefinition(d));
        this.jobDefinitions.setItems(
            jobDefs,
            (jobDef, itemView) => new JobDefinitionListItem(jobDef, itemView)
        );
        if (jobDefs.length === 0) {
            this.alert.danger("No job definitions were found");
        }
    }

    private getJobDefinitions() {
        return this.alert.infoAction(
            "Loading...",
            () => this.schdJobsClient.JobDefinitions.GetJobDefinitions()
        );
    }

    private onJobClicked(jobDefItem: JobDefinitionListItem) {
        this.awaitable.resolve(
            JobDefinitionListPanelResult.jobDefinitionSelected(jobDefItem.jobDefinition.id)
        );
    }

    refresh() { return this.refreshCommand.execute(); }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }

}