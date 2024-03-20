import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand, Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/Components/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/Components/MessageAlert";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { ScheduledJobsAppClient } from "../../../Lib/Http/ScheduledJobsAppClient";
import { EventSummaryListItem } from "./EventSummaryListItem";
import { EventSummaryListItemView } from "./EventSummaryListItemView";
import { NotificationListPanelView } from "./NotificationListPanelView";

interface IResults {
    menuRequested?: boolean;
}

export class NotificationListPanelResult {
    static menuRequested() { return new NotificationListPanelResult({ menuRequested: true }); }

    private constructor(private readonly results: IResults) { }

    get menuRequested() { return this.results.menuRequested; }
}

export class NotificationListPanel implements IPanel {
    private readonly awaitable = new Awaitable<NotificationListPanelResult>();
    private readonly alert: MessageAlert;
    private readonly recentEventsList: ListGroup<EventSummaryListItem, EventSummaryListItemView>;
    private readonly refreshCommand: AsyncCommand;

    constructor(
        private readonly schdJobsClient: ScheduledJobsAppClient,
        private readonly view: NotificationListPanelView
    ) {
        this.alert = new MessageAlert(view.alert);
        this.recentEventsList = new ListGroup(view.recentEventListView);
        new TextComponent(view.heading).setText('Events');
        new Command(this.requestMenu.bind(this)).add(view.menuButton);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.add(view.refreshButton);
        this.refreshCommand.animateIconWhenInProgress('spin');
    }

    private requestMenu() {
        this.awaitable.resolve(NotificationListPanelResult.menuRequested());
    }

    private async doRefresh() {
        let recentEvents = await this.getRecentEvents();
        this.recentEventsList.setItems(
            recentEvents,
            (evt, itemView) => new EventSummaryListItem(this.schdJobsClient, evt, itemView)
        );
        if (recentEvents.length === 0) {
            this.alert.danger('No events were found.');
        }
    }

    private getRecentEvents() {
        return this.alert.infoAction(
            'Loading...',
            () => this.schdJobsClient.EventInquiry.GetRecentNotifications()
        );
    }

    refresh() { return this.refreshCommand.execute(); }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }

}