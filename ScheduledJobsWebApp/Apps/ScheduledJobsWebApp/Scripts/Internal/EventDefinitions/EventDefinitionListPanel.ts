import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand, Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/Components/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/Components/MessageAlert";
import { ScheduledJobsAppClient } from "../../Lib/Http/ScheduledJobsAppClient";
import { EventDefinitionListItem } from "./EventDefinitionListItem";
import { EventDefinitionListItemView } from "./EventDefinitionListItemView";
import { EventDefinitionListPanelView } from "./EventDefinitionListPanelView";
import { CardAlert } from "@jasonbenfield/sharedwebapp/Components/CardAlert";

interface IResults {
    menuRequested?: boolean;
    eventDefinitionSelected?: { eventDefinitionID: number; }
}

export class EventDefinitionListPanelResult {
    static menuRequested() { return new EventDefinitionListPanelResult({ menuRequested: true }); }

    static eventDefinitionSelected(eventDefinitionID: number) {
        return new EventDefinitionListPanelResult({
            eventDefinitionSelected: { eventDefinitionID: eventDefinitionID }
        });
    }

    private constructor(private readonly results: IResults) { }

    get menuRequested() { return this.results.menuRequested; }

    get eventDefinitionSelected() { return this.results.eventDefinitionSelected; }
}

export class EventDefinitionListPanel implements IPanel {
    private readonly awaitable = new Awaitable<EventDefinitionListPanelResult>();
    private readonly alert: MessageAlert;
    private readonly eventDefinitions: ListGroup<EventDefinitionListItem, EventDefinitionListItemView>;
    private readonly refreshCommand: AsyncCommand;

    constructor(private readonly schdJobsClient: ScheduledJobsAppClient, private readonly view: EventDefinitionListPanelView) {
        this.alert = new CardAlert(view.alert).alert;
        this.eventDefinitions = new ListGroup(view.eventDefinitionListView);
        this.eventDefinitions.when.itemClicked.then(this.onDefinitionClicked.bind(this));
        new Command(this.menu.bind(this)).add(view.menuButton);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.add(view.refreshButton);
        this.refreshCommand.animateIconWhenInProgress('spin');
    }

    private onDefinitionClicked(defItem: EventDefinitionListItem) {
        this.awaitable.resolve(EventDefinitionListPanelResult.eventDefinitionSelected(defItem.evtDef.ID));
    }

    private menu() { this.awaitable.resolve(EventDefinitionListPanelResult.menuRequested()); }

    private async doRefresh() {
        const evtDefs = await this.getEventDefinitions();
        this.eventDefinitions.setItems(
            evtDefs,
            (evtDef, itemView) => new EventDefinitionListItem(evtDef, itemView)
        );
        if (evtDefs.length === 0) {
            this.alert.danger('No event definitions were found');
        }
    }

    private getEventDefinitions() {
        return this.alert.infoAction(
            'Loading...',
            () => this.schdJobsClient.EventDefinitions.GetEventDefinitions()
        );
    }

    refresh() { return this.refreshCommand.execute(); }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }
}