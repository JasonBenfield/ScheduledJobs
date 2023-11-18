// Generated code

import { AppClientGroup } from "@jasonbenfield/sharedwebapp/Http/AppClientGroup";
import { AppClientAction } from "@jasonbenfield/sharedwebapp/Http/AppClientAction";
import { AppClientView } from "@jasonbenfield/sharedwebapp/Http/AppClientView";
import { AppClientEvents } from "@jasonbenfield/sharedwebapp/Http/AppClientEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Http/AppResourceUrl";

export class EventsGroup extends AppClientGroup {
	constructor(events: AppClientEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'Events');
		this.AddOrUpdateRegisteredEventsAction = this.createAction<IRegisteredEvent[],IEmptyActionResult>('AddOrUpdateRegisteredEvents', 'Add Or Update Registered Events');
		this.AddNotificationsAction = this.createAction<IAddNotificationsRequest,IEventNotificationModel[]>('AddNotifications', 'Add Notifications');
		this.TriggeredJobsAction = this.createAction<ITriggeredJobsRequest,ITriggeredJobWithTasksModel[]>('TriggeredJobs', 'Triggered Jobs');
	}
	
	readonly AddOrUpdateRegisteredEventsAction: AppClientAction<IRegisteredEvent[],IEmptyActionResult>;
	readonly AddNotificationsAction: AppClientAction<IAddNotificationsRequest,IEventNotificationModel[]>;
	readonly TriggeredJobsAction: AppClientAction<ITriggeredJobsRequest,ITriggeredJobWithTasksModel[]>;
	
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