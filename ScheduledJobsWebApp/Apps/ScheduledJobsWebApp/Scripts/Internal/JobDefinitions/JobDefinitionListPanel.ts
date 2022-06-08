import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand } from "@jasonbenfield/sharedwebapp/Command/AsyncCommand";
import { Command } from "@jasonbenfield/sharedwebapp/Command/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/MessageAlert";
import { ScheduledJobsAppApi } from "../../ScheduledJobs/Api/ScheduledJobsAppApi";
import { JobDefinitionListItem } from "./JobDefinitionListItem";
import { JobDefinitionListItemView } from "./JobDefinitionListItemView";
import { JobDefinitionListPanelView } from "./JobDefinitionListPanelView";

interface IResults {
    menuRequested?: {};
    jobDefinitionSelected?: { jobDefinitionID: number; }
}

export class JobDefinitionListPanelResult {
    static menuRequested() { return new JobDefinitionListPanelResult({ menuRequested: {} }); }

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
    private readonly alert: MessageAlert;
    private readonly jobDefinitions: ListGroup;
    private readonly refreshCommand: AsyncCommand;

    constructor(private readonly schdJobsApi: ScheduledJobsAppApi, private readonly view: JobDefinitionListPanelView) {
        this.alert = new MessageAlert(view.alert);
        this.jobDefinitions = new ListGroup(view.jobDefinitions);
        this.jobDefinitions.itemClicked.register(this.onJobClicked.bind(this));
        new Command(this.requestMenu.bind(this)).add(view.menuButton);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.add(view.refreshButton);
        this.refreshCommand.animateIconWhenInProgress('spin');
    }

    private requestMenu() { this.awaitable.resolve(JobDefinitionListPanelResult.menuRequested()); }

    private async doRefresh() {
        let jobDefs = await this.getJobDefinitions();
        this.jobDefinitions.setItems(
            jobDefs,
            (jobDef, itemView: JobDefinitionListItemView) => new JobDefinitionListItem(jobDef, itemView)
        );
        if (jobDefs.length === 0) {
            this.alert.danger('No job definitions were found');
        }
    }

    private getJobDefinitions() {
        return this.alert.infoAction(
            'Loading...',
            () => this.schdJobsApi.JobDefinitions.GetJobDefinitions()
        );
    }

    private onJobClicked(jobDefItem: JobDefinitionListItem) {
        this.awaitable.resolve(JobDefinitionListPanelResult.jobDefinitionSelected(jobDefItem.jobDefinition.ID));
    }

    refresh() { return this.refreshCommand.execute(); }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }

}