import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { Command } from "@jasonbenfield/sharedwebapp/Command/Command";
import { ScheduledJobsAppApi } from "../ScheduledJobs/Api/ScheduledJobsAppApi";
import { MainMenuPanelView } from "./MainMenuPanelVIew";

interface IResults {
    done?: {};
}

export class MainMenuPanelResult {
    static done() { return new MainMenuPanelResult({ done: {} }); }

    private constructor(private readonly results: IResults) { }

    get done() { return this.results.done; }
}

export class MainMenuPanel implements IPanel {
    private readonly awaitable = new Awaitable<MainMenuPanelResult>();

    constructor(schdJobsApi: ScheduledJobsAppApi, private readonly view: MainMenuPanelView) {
        new Command(this.done.bind(this)).add(this.view.doneButton);
        this.view.eventDefinitionsLink.setHref(schdJobsApi.EventDefinitions.Index.getUrl({}).value());
        this.view.notificationsLink.setHref(schdJobsApi.EventInquiry.Notifications.getUrl({}).value());
        this.view.jobDefinitionsLink.setHref(schdJobsApi.JobDefinitions.Index.getUrl({}).value());
        this.view.failedJobsLink.setHref(schdJobsApi.JobInquiry.FailedJobs.getUrl({}).value());
    }

    private done() {
        this.awaitable.resolve(MainMenuPanelResult.done());
    }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }
}