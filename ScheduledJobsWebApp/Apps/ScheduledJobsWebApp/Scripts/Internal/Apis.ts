import { AppApiFactory } from "@jasonbenfield/sharedwebapp/Api/AppApiFactory";
import { ModalErrorComponent } from "@jasonbenfield/sharedwebapp/Error/ModalErrorComponent";
import { ModalErrorComponentView } from "@jasonbenfield/sharedwebapp/Error/ModalErrorComponentView";
import { ScheduledJobsAppApi } from "../ScheduledJobs/Api/ScheduledJobsAppApi";

export class Apis {
    private readonly modalError: ModalErrorComponent;

    constructor(modalError: ModalErrorComponentView) {
        this.modalError = new ModalErrorComponent(modalError);
    }

    ScheduledJobs() {
        let apiFactory = new AppApiFactory(this.modalError)
        return apiFactory.api(ScheduledJobsAppApi);
    }
}