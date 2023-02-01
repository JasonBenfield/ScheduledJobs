// Generated code

import { AppApiGroup } from "@jasonbenfield/sharedwebapp/Api/AppApiGroup";
import { AppApiAction } from "@jasonbenfield/sharedwebapp/Api/AppApiAction";
import { AppApiView } from "@jasonbenfield/sharedwebapp/Api/AppApiView";
import { AppApiEvents } from "@jasonbenfield/sharedwebapp/Api/AppApiEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Api/AppResourceUrl";

export class EventsGroup extends AppApiGroup {
	constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'Events');
		this.AddJobScheduleNotificationsAction = this.createAction<IEmptyRequest,IEmptyActionResult>('AddJobScheduleNotifications', 'Add Job Schedule Notifications');
		this.AddOrUpdateRegisteredEventsAction = this.createAction<IRegisteredEvent[],IEmptyActionResult>('AddOrUpdateRegisteredEvents', 'Add Or Update Registered Events');
		this.AddNotificationsAction = this.createAction<IAddNotificationsRequest,IEventNotificationModel[]>('AddNotifications', 'Add Notifications');
		this.TriggeredJobsAction = this.createAction<ITriggeredJobsRequest,ITriggeredJobWithTasksModel[]>('TriggeredJobs', 'Triggered Jobs');
	}
	
	readonly AddJobScheduleNotificationsAction: AppApiAction<IEmptyRequest,IEmptyActionResult>;
	readonly AddOrUpdateRegisteredEventsAction: AppApiAction<IRegisteredEvent[],IEmptyActionResult>;
	readonly AddNotificationsAction: AppApiAction<IAddNotificationsRequest,IEventNotificationModel[]>;
	readonly TriggeredJobsAction: AppApiAction<ITriggeredJobsRequest,ITriggeredJobWithTasksModel[]>;
	
	AddJobScheduleNotifications(errorOptions?: IActionErrorOptions) {
		return this.AddJobScheduleNotificationsAction.execute({}, errorOptions || {});
	}
	AddOrUpdateRegisteredEvents(model: IRegisteredEvent[], errorOptions?: IActionErrorOptions) {
		return this.AddOrUpdateRegisteredEventsAction.execute(model, errorOptions || {});
	}
	AddNotifications(model: IAddNotificationsRequest, errorOptions?: IActionErrorOptions) {
		return this.AddNotificationsAction.execute(model, errorOptions || {});
	}
	TriggeredJobs(model: ITriggeredJobsRequest, errorOptions?: IActionErrorOptions) {
		return this.TriggeredJobsAction.execute(model, errorOptions || {});
	}
}