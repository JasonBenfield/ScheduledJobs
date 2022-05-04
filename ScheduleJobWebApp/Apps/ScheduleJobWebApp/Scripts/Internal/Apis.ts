import { AppApiFactory } from "@jasonbenfield/sharedwebapp/Api/AppApiFactory";
import { ModalErrorComponent } from "@jasonbenfield/sharedwebapp/Error/ModalErrorComponent";
import { ModalErrorComponentView } from "@jasonbenfield/sharedwebapp/Error/ModalErrorComponentView";
import { ScheduleJobAppApi } from "../ScheduleJob/Api/ScheduleJobAppApi";

export class Apis {
    private readonly modalError: ModalErrorComponent;

    constructor(modalError: ModalErrorComponentView) {
        this.modalError = new ModalErrorComponent(modalError);
    }

    ScheduleJob() {
        let apiFactory = new AppApiFactory(this.modalError)
        return apiFactory.api(ScheduleJobAppApi);
    }
}