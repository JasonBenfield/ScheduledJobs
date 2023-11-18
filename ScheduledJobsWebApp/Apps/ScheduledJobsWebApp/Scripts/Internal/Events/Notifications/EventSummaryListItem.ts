import { BasicComponent } from "@jasonbenfield/sharedwebapp/Components/BasicComponent";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { FormattedDate } from "@jasonbenfield/sharedwebapp/FormattedDate";
import { EventSummaryListItemView } from "./EventSummaryListItemView";
import { ScheduledJobsAppClient } from "../../../Lib/Http/ScheduledJobsAppClient";

export class EventSummaryListItem extends BasicComponent {
    constructor(schdJobsClient: ScheduledJobsAppClient, evt: IEventSummaryModel, view: EventSummaryListItemView) {
        super(view);
        view.setHref(schdJobsClient.EventInquiry.NotificationDetail.getUrl({ NotificationID: evt.Event.ID }).value());
        new TextComponent(view.displayText).setText(evt.Event.Definition.EventKey.DisplayText);
        new TextComponent(view.timeActive).setText(new FormattedDate(evt.Event.TimeActive).formatDateTime());
        new TextComponent(view.sourceKey).setText(evt.Event.SourceKey);
        const sourceData = new TextComponent(view.sourceData);
        sourceData.setText(evt.Event.SourceData);
        sourceData.syncTitleWithText();
        new TextComponent(view.triggeredJobCount).setText(evt.TriggeredJobCount.toString());
    }
}