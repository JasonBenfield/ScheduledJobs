import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand } from "@jasonbenfield/sharedwebapp/Command/AsyncCommand";
import { Command } from "@jasonbenfield/sharedwebapp/Command/Command";
import { TextBlock } from "@jasonbenfield/sharedwebapp/Html/TextBlock";
import { ListGroup } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/MessageAlert";
import { ScheduledJobsAppApi } from "../../../ScheduledJobs/Api/ScheduledJobsAppApi";
import { EventSummaryListItem } from "./EventSummaryListItem";
import { EventSummaryListItemView } from "./EventSummaryListItemView";
import { NotificationListPanelView } from "./NotificationListPanelView";

interface IResults {
    menuRequested?: {};
}

export class NotificationListPanelResult {
    static menuRequested() { return new NotificationListPanelResult({ menuRequested: {} }); }

    private constructor(private readonly results: IResults) { }

    get menuRequested() { return this.results.menuRequested; }
}

export class NotificationListPanel implements IPanel {
    private readonly awaitable = new Awaitable<NotificationListPanelResult>();
    private readonly alert: MessageAlert;
    private readonly recentEventsList: ListGroup;
    private readonly refreshCommand: AsyncCommand;

    constructor(
        private readonly schdJobsApi: ScheduledJobsAppApi,
        private readonly view: NotificationListPanelView
    ) {
        this.alert = new MessageAlert(view.alert);
        this.recentEventsList = new ListGroup(view.recentEvents);
        new TextBlock('Events', view.heading);
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
            (evt, itemView: EventSummaryListItemView) => new EventSummaryListItem(this.schdJobsApi, evt, itemView)
        );
        if (recentEvents.length === 0) {
            this.alert.danger('No events were found.');
        }
    }

    private async getRecentEvents() {
        let recentEvents: IEventSummaryModel[];
        await this.alert.infoAction(
            'Loading...',
            async () => {
                recentEvents = await this.schdJobsApi.EventInquiry.GetRecentNotifications();
            }
        );
        return recentEvents;
    }

    refresh() { return this.refreshCommand.execute(); }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }

}