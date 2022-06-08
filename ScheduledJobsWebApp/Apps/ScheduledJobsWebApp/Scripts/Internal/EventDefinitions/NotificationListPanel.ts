﻿import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand } from "@jasonbenfield/sharedwebapp/Command/AsyncCommand";
import { Command } from "@jasonbenfield/sharedwebapp/Command/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/MessageAlert";
import { ScheduledJobsAppApi } from "../../ScheduledJobs/Api/ScheduledJobsAppApi";
import { EventSummaryListItem } from "../Events/Notifications/EventSummaryListItem";
import { EventSummaryListItemView } from "../Events/Notifications/EventSummaryListItemView";
import { NotificationListPanelView } from "./NotificationListPanelView";

interface IResults {
    back?: {};
}

export class NotificationListPanelResult {
    static back() { return new NotificationListPanelResult({ back: {} }); }

    private constructor(private readonly results: IResults) { }

    get back() { return this.results.back; }
}

export class NotificationListPanel implements IPanel {
    private readonly awaitable = new Awaitable<NotificationListPanelResult>();
    private readonly alert: MessageAlert;
    private readonly notifications: ListGroup;
    private readonly refreshCommand: AsyncCommand;
    private eventDefinitionID: number;
    private sourceKey: string = '';

    constructor(private readonly schdJobsApi: ScheduledJobsAppApi, private readonly view: NotificationListPanelView) {
        this.alert = new MessageAlert(view.alert);
        this.notifications = new ListGroup(view.notifications);
        new Command(this.back.bind(this)).add(view.backButton);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.add(view.refreshButton);
        this.refreshCommand.animateIconWhenInProgress('spin');
    }

    private back() { this.awaitable.resolve(NotificationListPanelResult.back()); }

    private async doRefresh() {
        let notifications = await this.getNotifications();
        this.notifications.setItems(
            notifications,
            (notification, itemView: EventSummaryListItemView) =>
                new EventSummaryListItem(this.schdJobsApi, notification, itemView)
        );
    }

    private getNotifications() {
        return this.alert.infoAction(
            'Loading',
            () => this.schdJobsApi.EventDefinitions.GetRecentNotifications({
                EventDefinitionID: this.eventDefinitionID,
                SourceKey: this.sourceKey
            })
        )
    }

    setEventDefinitionID(eventDefinitionID: number) { this.eventDefinitionID = eventDefinitionID; }

    setSourceKey(sourceKey: string) { this.sourceKey = sourceKey; }

    refresh() { return this.refreshCommand.execute(); }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }
}