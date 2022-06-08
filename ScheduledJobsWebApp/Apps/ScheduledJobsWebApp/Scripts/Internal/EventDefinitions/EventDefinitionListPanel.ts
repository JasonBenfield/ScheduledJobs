﻿import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand } from "@jasonbenfield/sharedwebapp/Command/AsyncCommand";
import { ListGroup } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/MessageAlert";
import { ScheduledJobsAppApi } from "../../ScheduledJobs/Api/ScheduledJobsAppApi";
import { EventDefinitionListItem } from "./EventDefinitionListItem";
import { EventDefinitionListItemView } from "./EventDefinitionListItemView";
import { EventDefinitionListPanelView } from "./EventDefinitionListPanelView";

interface IResults {
    menuRequested?: {};
    eventDefinitionSelected?: { eventDefinitionID: number; }
}

export class EventDefinitionListPanelResult {
    static menuRequested() { return new EventDefinitionListPanelResult({ menuRequested: {} }); }

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
    private readonly eventDefinitions: ListGroup;
    private readonly refreshCommand: AsyncCommand;

    constructor(private readonly schdJobsApi: ScheduledJobsAppApi, private readonly view: EventDefinitionListPanelView) {
        this.alert = new MessageAlert(view.alert);
        this.eventDefinitions = new ListGroup(view.eventDefinitions);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.add(view.refreshButton);
        this.refreshCommand.animateIconWhenInProgress('spin');
    }

    private async doRefresh() {
        let evtDefs = await this.getEventDefinitions();
        this.eventDefinitions.setItems(
            evtDefs,
            (evtDef, itemView: EventDefinitionListItemView) => new EventDefinitionListItem(evtDef, itemView)
        );
        if (evtDefs.length === 0) {
            this.alert.danger('No event definitions were found');
        }
    }

    private getEventDefinitions() {
        return this.alert.infoAction(
            'Loading...',
            () => this.schdJobsApi.EventDefinitions.GetEventDefinitions()
        );
    }

    refresh() { return this.refreshCommand.execute(); }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }
}