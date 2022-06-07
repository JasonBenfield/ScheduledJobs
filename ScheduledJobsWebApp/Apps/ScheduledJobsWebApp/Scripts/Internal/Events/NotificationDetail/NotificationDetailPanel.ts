import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand } from "@jasonbenfield/sharedwebapp/Command/AsyncCommand";
import { Command } from "@jasonbenfield/sharedwebapp/Command/Command";
import { TextBlock } from "@jasonbenfield/sharedwebapp/Html/TextBlock";
import { ListGroup } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/MessageAlert";
import { ScheduledJobsAppApi } from "../../../ScheduledJobs/Api/ScheduledJobsAppApi";
import { JobSummaryListItem } from "../../Jobs/JobSummaryListItem";
import { JobSummaryListItemView } from "../../Jobs/JobSummaryListItemView";
import { NotificationDetailPanelView } from "./NotificationDetailPanelView";

interface IResults {
    menuRequested?: {};
}

export class NotificationDetailPanelResult {
    static menuRequested() { return new NotificationDetailPanelResult({ menuRequested: {} }); }

    private constructor(private readonly results: IResults) { }

    get menuRequested() { return this.results.menuRequested; }
}

export class NotificationDetailPanel implements IPanel {
    private readonly awaitable = new Awaitable<NotificationDetailPanelResult>();
    private readonly triggeredJobs: ListGroup;
    private readonly alert: MessageAlert;
    private readonly refreshCommand: AsyncCommand;
    private notificationID: number;

    constructor(private readonly schdJobsApi: ScheduledJobsAppApi, private readonly view: NotificationDetailPanelView) {
        this.alert = new MessageAlert(this.view.alert);
        this.view.hideJobDetail();
        new TextBlock('Triggered Jobs', this.view.triggeredJobsTitle);
        this.triggeredJobs = new ListGroup(this.view.triggeredJobs);
        new Command(this.requestMenu.bind(this)).add(view.menuButton);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.animateIconWhenInProgress('spin');
    }

    private requestMenu() {
        this.awaitable.resolve(NotificationDetailPanelResult.menuRequested());
    }

    private async doRefresh() {
        let notificationDetail = await this.getNotificationDetail(this.notificationID);
        new TextBlock(notificationDetail.Event.Definition.EventKey.DisplayText, this.view.eventDisplayText);
        this.triggeredJobs.setItems(
            notificationDetail.TriggeredJobs,
            (job, itemView: JobSummaryListItemView) => new JobSummaryListItem(this.schdJobsApi, job, itemView)
        );
        this.view.showJobDetail();
    }

    private async getNotificationDetail(notificationID: number) {
        let notificationDetail: IEventNotificationDetailModel;
        await this.alert.infoAction(
            'Loading...',
            async () => {
                notificationDetail = await this.schdJobsApi.EventInquiry.GetNotificationDetail(
                    { NotificationID: notificationID }
                );
            }
        );
        return notificationDetail;
    }

    setNotificationID(notificationID: number) {
        this.notificationID = notificationID;
    }

    refresh() { return this.refreshCommand.execute(); }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }

}