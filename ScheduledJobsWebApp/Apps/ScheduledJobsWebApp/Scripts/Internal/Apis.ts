import { AppApiFactory } from "@jasonbenfield/sharedwebapp/Api/AppApiFactory";
import { ModalErrorView } from "@jasonbenfield/sharedwebapp/Views/ModalError";
import { ScheduledJobsAppApi } from "../Lib/Api/ScheduledJobsAppApi";

export class Apis {
    constructor(private readonly modalError: ModalErrorView) {
    }

    ScheduledJobs() {
        const apiFactory = new AppApiFactory(this.modalError)
        return apiFactory.api(ScheduledJobsAppApi);
    }
}