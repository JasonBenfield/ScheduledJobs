import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand, Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { ListGroup } from "@jasonbenfield/sharedwebapp/Components/ListGroup";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/Components/MessageAlert";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { FormGroupText } from "@jasonbenfield/sharedwebapp/Forms/FormGroupText";
import { EventNotificationDetail } from "../../../Lib/EventNotificationDetail";
import { ScheduledJobsAppClient } from "../../../Lib/Http/ScheduledJobsAppClient";
import { JobSummaryListItem } from "../../Jobs/JobSummaryListItem";
import { JobSummaryListItemView } from "../../Jobs/JobSummaryListItemView";
import { NotificationDetailPanelView } from "./NotificationDetailPanelView";

interface IResults {
    menuRequested?: boolean;
}

export class NotificationDetailPanelResult {
    static menuRequested() { return new NotificationDetailPanelResult({ menuRequested: true }); }

    private constructor(private readonly results: IResults) { }

    get menuRequested() { return this.results.menuRequested; }
}

export class NotificationDetailPanel implements IPanel {
    private readonly awaitable = new Awaitable<NotificationDetailPanelResult>();
    private readonly sourceKey: FormGroupText;
    private readonly sourceData: FormGroupText;
    private readonly triggeredJobs: ListGroup<JobSummaryListItem, JobSummaryListItemView>;
    private readonly alert: MessageAlert;
    private readonly refreshCommand: AsyncCommand;
    private notificationID: number;
    private readonly eventDisplayText: TextComponent;

    constructor(private readonly schdJobsClient: ScheduledJobsAppClient, private readonly view: NotificationDetailPanelView) {
        this.alert = new MessageAlert(this.view.alert);
        this.sourceKey = new FormGroupText(view.sourceKey);
        this.sourceKey.setCaption("Source Key");
        this.sourceData = new FormGroupText(view.sourceData);
        this.view.hideJobDetail();
        new TextComponent(this.view.triggeredJobsTitle).setText("Triggered Jobs");
        this.eventDisplayText = new TextComponent(this.view.eventDisplayText);
        this.triggeredJobs = new ListGroup(this.view.triggeredJobListView);
        new Command(this.requestMenu.bind(this)).add(view.menuButton);
        this.refreshCommand = new AsyncCommand(this.doRefresh.bind(this));
        this.refreshCommand.add(view.refreshButton);
        this.refreshCommand.animateIconWhenInProgress("spin");
    }

    private requestMenu() {
        this.awaitable.resolve(NotificationDetailPanelResult.menuRequested());
    }

    private async doRefresh() {
        const sourceNotificationDetail = await this.getNotificationDetail(this.notificationID);
        const notificationDetail = new EventNotificationDetail(sourceNotificationDetail);
        this.eventDisplayText.setText(notificationDetail.event.definition.eventKey.displayText);
        this.sourceKey.setValue(notificationDetail.event.sourceKey);
        this.sourceData.setValue(notificationDetail.event.sourceData);
        this.triggeredJobs.setItems(
            notificationDetail.triggeredJobs,
            (job, itemView) => new JobSummaryListItem(this.schdJobsClient, job, itemView)
        );
        this.view.showJobDetail();
    }

    private getNotificationDetail(notificationID: number) {
        return this.alert.infoAction(
            "Loading...",
            () => this.schdJobsClient.EventInquiry.GetNotificationDetail(
                { NotificationID: notificationID }
            )
        );
    }

    setNotificationID(notificationID: number) {
        this.notificationID = notificationID;
    }

    refresh() { return this.refreshCommand.execute(); }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }

}