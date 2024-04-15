// Generated code

import * as xti from "@jasonbenfield/sharedwebapp/Common";
import { AppClientGroup } from "@jasonbenfield/sharedwebapp/Http/AppClientGroup";
import { AppClientAction } from "@jasonbenfield/sharedwebapp/Http/AppClientAction";
import { AppClientView } from "@jasonbenfield/sharedwebapp/Http/AppClientView";
import { AppClientEvents } from "@jasonbenfield/sharedwebapp/Http/AppClientEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Http/AppResourceUrl";

export class RecurringGroup extends AppClientGroup {
	constructor(events: AppClientEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'Recurring');
		this.AddJobScheduleNotificationsAction = this.createAction<IEmptyRequest,IEmptyActionResult>('AddJobScheduleNotifications', 'Add Job Schedule Notifications');
		this.TimeoutTasksAction = this.createAction<IEmptyRequest,IEmptyActionResult>('TimeoutTasks', 'Timeout Tasks');
		this.PurgeJobsAndEventsAction = this.createAction<IEmptyRequest,IEmptyActionResult>('PurgeJobsAndEvents', 'Purge Jobs And Events');
	}
	
	readonly AddJobScheduleNotificationsAction: AppClientAction<IEmptyRequest,IEmptyActionResult>;
	readonly TimeoutTasksAction: AppClientAction<IEmptyRequest,IEmptyActionResult>;
	readonly PurgeJobsAndEventsAction: AppClientAction<IEmptyRequest,IEmptyActionResult>;
	
	AddJobScheduleNotifications(errorOptions?: IActionErrorOptions) {
		return this.AddJobScheduleNotificationsAction.execute({}, errorOptions || {});
	}
	TimeoutTasks(errorOptions?: IActionErrorOptions) {
		return this.TimeoutTasksAction.execute({}, errorOptions || {});
	}
	PurgeJobsAndEvents(errorOptions?: IActionErrorOptions) {
		return this.PurgeJobsAndEventsAction.execute({}, errorOptions || {});
	}
}