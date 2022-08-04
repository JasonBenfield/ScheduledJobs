// Generated code

import { AppApiGroup } from "@jasonbenfield/sharedwebapp/Api/AppApiGroup";
import { AppApiAction } from "@jasonbenfield/sharedwebapp/Api/AppApiAction";
import { AppApiView } from "@jasonbenfield/sharedwebapp/Api/AppApiView";
import { AppApiEvents } from "@jasonbenfield/sharedwebapp/Api/AppApiEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Api/AppResourceUrl";

export class RecurringGroup extends AppApiGroup {
	constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'Recurring');
		this.TimeoutTasksAction = this.createAction<IEmptyRequest,IEmptyActionResult>('TimeoutTasks', 'Timeout Tasks');
		this.PurgeJobsAndEventsAction = this.createAction<IEmptyRequest,IEmptyActionResult>('PurgeJobsAndEvents', 'Purge Jobs And Events');
	}
	
	readonly TimeoutTasksAction: AppApiAction<IEmptyRequest,IEmptyActionResult>;
	readonly PurgeJobsAndEventsAction: AppApiAction<IEmptyRequest,IEmptyActionResult>;
	
	TimeoutTasks(errorOptions?: IActionErrorOptions) {
		return this.TimeoutTasksAction.execute({}, errorOptions || {});
	}
	PurgeJobsAndEvents(errorOptions?: IActionErrorOptions) {
		return this.PurgeJobsAndEventsAction.execute({}, errorOptions || {});
	}
}