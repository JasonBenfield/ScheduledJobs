import { BasicPage } from "@jasonbenfield/sharedwebapp/Components/BasicPage";
import { BasicPageView } from "@jasonbenfield/sharedwebapp/Views/BasicPageView";
import { ScheduledJobsAppApi } from "../Lib/Api/ScheduledJobsAppApi";
import { Apis } from "./Apis";

export class ScheduledJobsPage extends BasicPage {
    protected readonly defaultApi: ScheduledJobsAppApi;

    constructor(view: BasicPageView) {
        super(new Apis(view.modalError).ScheduledJobs(), view);
    }
}