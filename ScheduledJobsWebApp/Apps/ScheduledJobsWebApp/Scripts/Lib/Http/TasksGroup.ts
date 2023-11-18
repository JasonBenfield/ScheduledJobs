// Generated code

import { AppClientGroup } from "@jasonbenfield/sharedwebapp/Http/AppClientGroup";
import { AppClientAction } from "@jasonbenfield/sharedwebapp/Http/AppClientAction";
import { AppClientView } from "@jasonbenfield/sharedwebapp/Http/AppClientView";
import { AppClientEvents } from "@jasonbenfield/sharedwebapp/Http/AppClientEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Http/AppResourceUrl";

export class TasksGroup extends AppClientGroup {
	constructor(events: AppClientEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'Tasks');
		this.CancelTaskAction = this.createAction<IGetTaskRequest,IEmptyActionResult>('CancelTask', 'Cancel Task');
		this.RetryTaskAction = this.createAction<IGetTaskRequest,IEmptyActionResult>('RetryTask', 'Retry Task');
		this.SkipTaskAction = this.createAction<IGetTaskRequest,IEmptyActionResult>('SkipTask', 'Skip Task');
		this.TimeoutTaskAction = this.createAction<IGetTaskRequest,IEmptyActionResult>('TimeoutTask', 'Timeout Task');
		this.EditTaskDataAction = this.createAction<IEditTaskDataRequest,IEmptyActionResult>('EditTaskData', 'Edit Task Data');
	}
	
	readonly CancelTaskAction: AppClientAction<IGetTaskRequest,IEmptyActionResult>;
	readonly RetryTaskAction: AppClientAction<IGetTaskRequest,IEmptyActionResult>;
	readonly SkipTaskAction: AppClientAction<IGetTaskRequest,IEmptyActionResult>;
	readonly TimeoutTaskAction: AppClientAction<IGetTaskRequest,IEmptyActionResult>;
	readonly EditTaskDataAction: AppClientAction<IEditTaskDataRequest,IEmptyActionResult>;
	
	CancelTask(model: IGetTaskRequest, errorOptions?: IActionErrorOptions) {
		return this.CancelTaskAction.execute(model, errorOptions || {});
	}
	RetryTask(model: IGetTaskRequest, errorOptions?: IActionErrorOptions) {
		return this.RetryTaskAction.execute(model, errorOptions || {});
	}
	SkipTask(model: IGetTaskRequest, errorOptions?: IActionErrorOptions) {
		return this.SkipTaskAction.execute(model, errorOptions || {});
	}
	TimeoutTask(model: IGetTaskRequest, errorOptions?: IActionErrorOptions) {
		return this.TimeoutTaskAction.execute(model, errorOptions || {});
	}
	EditTaskData(model: IEditTaskDataRequest, errorOptions?: IActionErrorOptions) {
		return this.EditTaskDataAction.execute(model, errorOptions || {});
	}
}