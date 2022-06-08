import { FormattedDate } from "@jasonbenfield/sharedwebapp/FormattedDate";
import { TextBlock } from "@jasonbenfield/sharedwebapp/Html/TextBlock";
import { ScheduledJobsAppApi } from "../../../ScheduledJobs/Api/ScheduledJobsAppApi";
import { EventSummaryListItemView } from "./EventSummaryListItemView";

export class EventSummaryListItem {
    constructor(schdJobsApp: ScheduledJobsAppApi, evt: IEventSummaryModel, view: EventSummaryListItemView) {
        view.setHref(schdJobsApp.EventInquiry.NotificationDetail.getUrl({ NotificationID: evt.Event.ID }).value());
        new TextBlock(evt.Event.Definition.EventKey.DisplayText, view.displayText);
        new TextBlock(new FormattedDate(evt.Event.TimeActive).formatDateTime(), view.timeActive);
        new TextBlock(evt.Event.SourceKey, view.sourceKey);
        let sourceData = new TextBlock(evt.Event.SourceData, view.sourceData);
        sourceData.syncTitleWithText();
        new TextBlock(evt.TriggeredJobCount.toString(), view.triggeredJobCount);
    }
}