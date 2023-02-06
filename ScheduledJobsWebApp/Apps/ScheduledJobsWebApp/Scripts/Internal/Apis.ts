import { HubAppApi } from "@jasonbenfield/hubwebapp/Api/HubAppApi";
import { AppApiFactory } from "@jasonbenfield/sharedwebapp/Api/AppApiFactory";
import { ModalErrorView } from "@jasonbenfield/sharedwebapp/Views/ModalError";
import { ScheduledJobsAppApi } from "../Lib/Api/ScheduledJobsAppApi";

export class Apis {
    private readonly apiFactory: AppApiFactory;

    constructor(modalError: ModalErrorView) {
        this.apiFactory = new AppApiFactory(modalError);
    }

    ScheduledJobs() {
        return this.apiFactory.api(ScheduledJobsAppApi);
    }

    Hub() {
        return this.apiFactory.api(HubAppApi);
    }
}