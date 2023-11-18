import { BasicPage } from "@jasonbenfield/sharedwebapp/Components/BasicPage";
import { BasicPageView } from "@jasonbenfield/sharedwebapp/Views/BasicPageView";
import { ScheduledJobsAppClient } from "../Lib/Http/ScheduledJobsAppClient";
import { AppClients } from "./AppClients";

export class ScheduledJobsPage extends BasicPage {
    protected readonly schdJobsClient: ScheduledJobsAppClient;

    constructor(view: BasicPageView) {
        const schdJobsClient = new AppClients(view.modalError).ScheduledJobs();
        super(schdJobsClient, view);
        this.schdJobsClient = schdJobsClient;
    }
}