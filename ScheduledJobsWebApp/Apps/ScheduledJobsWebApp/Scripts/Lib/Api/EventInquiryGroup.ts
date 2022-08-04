// Generated code

import { AppApiGroup } from "@jasonbenfield/sharedwebapp/Api/AppApiGroup";
import { AppApiAction } from "@jasonbenfield/sharedwebapp/Api/AppApiAction";
import { AppApiView } from "@jasonbenfield/sharedwebapp/Api/AppApiView";
import { AppApiEvents } from "@jasonbenfield/sharedwebapp/Api/AppApiEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Api/AppResourceUrl";

export class EventInquiryGroup extends AppApiGroup {
	constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'EventInquiry');
		this.Notifications = this.createView<IEmptyRequest>('Notifications');
		this.GetRecentNotificationsAction = this.createAction<IEmptyRequest,IEventSummaryModel[]>('GetRecentNotifications', 'Get Recent Notifications');
		this.NotificationDetail = this.createView<IGetNotificationDetailRequest>('NotificationDetail');
		this.GetNotificationDetailAction = this.createAction<IGetNotificationDetailRequest,IEventNotificationDetailModel>('GetNotificationDetail', 'Get Notification Detail');
	}
	
	readonly Notifications: AppApiView<IEmptyRequest>;
	readonly GetRecentNotificationsAction: AppApiAction<IEmptyRequest,IEventSummaryModel[]>;
	readonly NotificationDetail: AppApiView<IGetNotificationDetailRequest>;
	readonly GetNotificationDetailAction: AppApiAction<IGetNotificationDetailRequest,IEventNotificationDetailModel>;
	
	GetRecentNotifications(errorOptions?: IActionErrorOptions) {
		return this.GetRecentNotificationsAction.execute({}, errorOptions || {});
	}
	GetNotificationDetail(model: IGetNotificationDetailRequest, errorOptions?: IActionErrorOptions) {
		return this.GetNotificationDetailAction.execute(model, errorOptions || {});
	}
}