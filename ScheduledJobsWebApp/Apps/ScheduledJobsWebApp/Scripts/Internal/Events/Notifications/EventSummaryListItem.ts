import { FormattedDate } from "@jasonbenfield/sharedwebapp/FormattedDate";
import { TextBlock } from "@jasonbenfield/sharedwebapp/Html/TextBlock";
import { ScheduledJobsAppApi } from "../../../ScheduledJobs/Api/ScheduledJobsAppApi";
import { EventSummaryListItemView } from "./EventSummaryListItemView";

export class EventSummaryListItem {
    constructor(schdJobsApp: ScheduledJobsAppApi, evt: IEventSummaryModel, itemView: EventSummaryListItemView) {
        itemView.setHref(schdJobsApp.EventInquiry.NotificationDetail.getUrl({ NotificationID: evt.Event.ID }).value());
        new TextBlock(evt.Event.Definition.EventKey.DisplayText, itemView.displayText);
        new TextBlock(new FormattedDate(evt.Event.TimeActive).formatDateTime(), itemView.timeActive);
        new TextBlock(evt.TriggeredJobCount.toString(), itemView.triggeredJobCount);
    }
}