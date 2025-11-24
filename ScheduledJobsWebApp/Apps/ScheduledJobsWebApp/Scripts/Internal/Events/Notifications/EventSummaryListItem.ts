import { BasicComponent } from "@jasonbenfield/sharedwebapp/Components/BasicComponent";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { EventSummary } from "../../../Lib/EventSummary";
import { ScheduledJobsAppClient } from "../../../Lib/Http/ScheduledJobsAppClient";
import { EventSummaryListItemView } from "./EventSummaryListItemView";

export class EventSummaryListItem extends BasicComponent {
    constructor(schdJobsClient: ScheduledJobsAppClient, evt: EventSummary, view: EventSummaryListItemView) {
        super(view);
        view.setHref(schdJobsClient.EventInquiry.NotificationDetail.getUrl({ NotificationID: evt.event.id }).value());
        new TextComponent(view.displayText).setText(evt.event.definition.eventKey.displayText);
        new TextComponent(view.timeActive).setText(evt.event.timeActive.format());
        new TextComponent(view.sourceKey).setText(evt.event.sourceKey);
        const sourceData = new TextComponent(view.sourceData);
        sourceData.setText(evt.event.sourceData);
        sourceData.syncTitleWithText();
        new TextComponent(view.triggeredJobCount).setText(evt.triggeredJobCount.toString());
    }
}