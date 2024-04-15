import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand, Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/Components/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/Components/MessageAlert";
import { ScheduledJobsAppClient } from "../../Lib/Http/ScheduledJobsAppClient";
import { EventSummaryListItem } from "../Events/Notifications/EventSummaryListItem";
import { EventSummaryListItemView } from "../Events/Notifications/EventSummaryListItemView";
import { NotificationListPanelView } from "./NotificationListPanelView";
import { CardAlert } from "@jasonbenfield/sharedwebapp/Components/CardAlert";

interface IResults {
    back?: boolean;
}

export class NotificationListPanelResult {
    static back() { return new NotificationListPanelResult({ back: true }); }

    private constructor(private readonly results: IResults) { }

    get back() { return this.results.back; }
}

export class NotificationListPanel implements IPanel {
    private readonly awaitable = new Awaitable<NotificationListPanelResult>();
    private readonly alert: MessageAlert;
    private readonly notifications: ListGroup<EventSummaryListItem, EventSummaryListItemView>;
    private readonly refreshCommand: AsyncCommand;
    private eventDefinitionID: number;
    private sourceKey: string = '';

    constructor(private readonly schdJobsClient: ScheduledJobsAppClient, private readonly view: NotificationListPanelView) {
        this.alert = new CardAlert(view.alert).alert;
        this.notifications = new ListGroup(view.notificationListView);
        new Command(this.back.bind(this)).add(view.backButton);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.add(view.refreshButton);
        this.refreshCommand.animateIconWhenInProgress('spin');
    }

    private back() { this.awaitable.resolve(NotificationListPanelResult.back()); }

    private async doRefresh() {
        const notifications = await this.getNotifications();
        this.notifications.setItems(
            notifications,
            (notification, itemView) =>
                new EventSummaryListItem(this.schdJobsClient, notification, itemView)
        );
        if (notifications.length === 0) {
            this.alert.danger('No Event Notifications were found.');
        }
    }

    private getNotifications() {
        return this.alert.infoAction(
            'Loading',
            () => this.schdJobsClient.EventDefinitions.GetRecentNotifications({
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