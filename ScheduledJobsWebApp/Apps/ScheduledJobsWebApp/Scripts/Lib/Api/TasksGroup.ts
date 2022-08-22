// Generated code

import { AppApiGroup } from "@jasonbenfield/sharedwebapp/Api/AppApiGroup";
import { AppApiAction } from "@jasonbenfield/sharedwebapp/Api/AppApiAction";
import { AppApiView } from "@jasonbenfield/sharedwebapp/Api/AppApiView";
import { AppApiEvents } from "@jasonbenfield/sharedwebapp/Api/AppApiEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Api/AppResourceUrl";

export class TasksGroup extends AppApiGroup {
	constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'Tasks');
		this.CancelTaskAction = this.createAction<IGetTaskRequest,IEmptyActionResult>('CancelTask', 'Cancel Task');
		this.RetryTaskAction = this.createAction<IGetTaskRequest,IEmptyActionResult>('RetryTask', 'Retry Task');
		this.TimeoutTaskAction = this.createAction<IGetTaskRequest,IEmptyActionResult>('TimeoutTask', 'Timeout Task');
		this.EditTaskDataAction = this.createAction<IEditTaskDataRequest,IEmptyActionResult>('EditTaskData', 'Edit Task Data');
	}
	
	readonly CancelTaskAction: AppApiAction<IGetTaskRequest,IEmptyActionResult>;
	readonly RetryTaskAction: AppApiAction<IGetTaskRequest,IEmptyActionResult>;
	readonly TimeoutTaskAction: AppApiAction<IGetTaskRequest,IEmptyActionResult>;
	readonly EditTaskDataAction: AppApiAction<IEditTaskDataRequest,IEmptyActionResult>;
	
	CancelTask(model: IGetTaskRequest, errorOptions?: IActionErrorOptions) {
		return this.CancelTaskAction.execute(model, errorOptions || {});
	}
	RetryTask(model: IGetTaskRequest, errorOptions?: IActionErrorOptions) {
		return this.RetryTaskAction.execute(model, errorOptions || {});
	}
	TimeoutTask(model: IGetTaskRequest, errorOptions?: IActionErrorOptions) {
		return this.TimeoutTaskAction.execute(model, errorOptions || {});
	}
	EditTaskData(model: IEditTaskDataRequest, errorOptions?: IActionErrorOptions) {
		return this.EditTaskDataAction.execute(model, errorOptions || {});
	}
}