import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { MenuComponent } from "@jasonbenfield/sharedwebapp/Components/MenuComponent";
import { ScheduledJobsAppClient } from "../Lib/Http/ScheduledJobsAppClient";
import { MainMenuPanelView } from "./MainMenuPanelVIew";

interface IResults {
    done?: boolean;
}

export class MainMenuPanelResult {
    static done() { return new MainMenuPanelResult({ done: true }); }

    private constructor(private readonly results: IResults) { }

    get done() { return this.results.done; }
}

export class MainMenuPanel implements IPanel {
    private readonly awaitable = new Awaitable<MainMenuPanelResult>();

    constructor(schdJobsClient: ScheduledJobsAppClient, private readonly view: MainMenuPanelView) {
        const menu = new MenuComponent(schdJobsClient, 'main', view.menu);
        menu.refresh();
        new Command(this.done.bind(this)).add(this.view.doneButton);
    }

    private done() {
        this.awaitable.resolve(MainMenuPanelResult.done());
    }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }
}