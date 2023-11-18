import { HubAppClient } from "@jasonbenfield/hubwebapp/Http/HubAppClient";
import { AppClientFactory } from "@jasonbenfield/sharedwebapp/Http/AppClientFactory";
import { ModalErrorView } from "@jasonbenfield/sharedwebapp/Views/ModalError";
import { ScheduledJobsAppClient } from "../Lib/Http/ScheduledJobsAppClient";

export class AppClients {
    private readonly appClientFactory: AppClientFactory;

    constructor(modalError: ModalErrorView) {
        this.appClientFactory = new AppClientFactory(modalError);
    }

    ScheduledJobs() {
        return this.appClientFactory.create(ScheduledJobsAppClient);
    }

    Hub() {
        return this.appClientFactory.create(HubAppClient);
    }
}