import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { MenuComponent } from "@jasonbenfield/sharedwebapp/Components/MenuComponent";
import { ScheduledJobsAppApi } from "../Lib/Api/ScheduledJobsAppApi";
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

    constructor(schdJobsApi: ScheduledJobsAppApi, private readonly view: MainMenuPanelView) {
        const menu = new MenuComponent(schdJobsApi, 'main', view.menu);
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