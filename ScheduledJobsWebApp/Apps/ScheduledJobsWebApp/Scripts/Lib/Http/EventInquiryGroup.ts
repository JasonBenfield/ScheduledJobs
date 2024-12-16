// Generated code

import * as xti from "@jasonbenfield/sharedwebapp/Common";
import { AppClientGroup } from "@jasonbenfield/sharedwebapp/Http/AppClientGroup";
import { AppClientAction } from "@jasonbenfield/sharedwebapp/Http/AppClientAction";
import { AppClientView } from "@jasonbenfield/sharedwebapp/Http/AppClientView";
import { AppClientEvents } from "@jasonbenfield/sharedwebapp/Http/AppClientEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Http/AppResourceUrl";

export class EventInquiryGroup extends AppClientGroup {
	constructor(events: AppClientEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'EventInquiry');
		this.GetNotificationDetailAction = this.createAction<IGetNotificationDetailRequest,IEventNotificationDetailModel>('GetNotificationDetail', 'Get Notification Detail');
		this.GetRecentNotificationsAction = this.createAction<IEmptyRequest,IEventSummaryModel[]>('GetRecentNotifications', 'Get Recent Notifications');
		this.NotificationDetail = this.createView<IGetNotificationDetailRequest>('NotificationDetail');
		this.Notifications = this.createView<IEmptyRequest>('Notifications');
	}
	
	readonly GetNotificationDetailAction: AppClientAction<IGetNotificationDetailRequest,IEventNotificationDetailModel>;
	readonly GetRecentNotificationsAction: AppClientAction<IEmptyRequest,IEventSummaryModel[]>;
	readonly NotificationDetail: AppClientView<IGetNotificationDetailRequest>;
	readonly Notifications: AppClientView<IEmptyRequest>;
	
	GetNotificationDetail(model: IGetNotificationDetailRequest, errorOptions?: IActionErrorOptions) {
		return this.GetNotificationDetailAction.execute(model, errorOptions || {});
	}
	GetRecentNotifications(errorOptions?: IActionErrorOptions) {
		return this.GetRecentNotificationsAction.execute({}, errorOptions || {});
	}
}