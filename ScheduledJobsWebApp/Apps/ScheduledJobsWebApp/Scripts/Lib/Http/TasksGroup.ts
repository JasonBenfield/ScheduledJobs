// Generated code

import * as xti from "@jasonbenfield/sharedwebapp/Common";
import { AppClientGroup } from "@jasonbenfield/sharedwebapp/Http/AppClientGroup";
import { AppClientAction } from "@jasonbenfield/sharedwebapp/Http/AppClientAction";
import { AppClientView } from "@jasonbenfield/sharedwebapp/Http/AppClientView";
import { AppClientEvents } from "@jasonbenfield/sharedwebapp/Http/AppClientEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Http/AppResourceUrl";

export class TasksGroup extends AppClientGroup {
	constructor(events: AppClientEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'Tasks');
		this.CancelTaskAction = this.createAction<IGetTaskRequest,IEmptyActionResult>('CancelTask', 'Cancel Task');
		this.EditTaskDataAction = this.createAction<IEditTaskDataRequest,IEmptyActionResult>('EditTaskData', 'Edit Task Data');
		this.RetryTaskAction = this.createAction<IGetTaskRequest,IEmptyActionResult>('RetryTask', 'Retry Task');
		this.SkipTaskAction = this.createAction<IGetTaskRequest,IEmptyActionResult>('SkipTask', 'Skip Task');
		this.TimeoutTaskAction = this.createAction<IGetTaskRequest,IEmptyActionResult>('TimeoutTask', 'Timeout Task');
	}
	
	readonly CancelTaskAction: AppClientAction<IGetTaskRequest,IEmptyActionResult>;
	readonly EditTaskDataAction: AppClientAction<IEditTaskDataRequest,IEmptyActionResult>;
	readonly RetryTaskAction: AppClientAction<IGetTaskRequest,IEmptyActionResult>;
	readonly SkipTaskAction: AppClientAction<IGetTaskRequest,IEmptyActionResult>;
	readonly TimeoutTaskAction: AppClientAction<IGetTaskRequest,IEmptyActionResult>;
	
	CancelTask(requestData: IGetTaskRequest, errorOptions?: IActionErrorOptions) {
		return this.CancelTaskAction.execute(requestData, errorOptions || {});
	}
	EditTaskData(requestData: IEditTaskDataRequest, errorOptions?: IActionErrorOptions) {
		return this.EditTaskDataAction.execute(requestData, errorOptions || {});
	}
	RetryTask(requestData: IGetTaskRequest, errorOptions?: IActionErrorOptions) {
		return this.RetryTaskAction.execute(requestData, errorOptions || {});
	}
	SkipTask(requestData: IGetTaskRequest, errorOptions?: IActionErrorOptions) {
		return this.SkipTaskAction.execute(requestData, errorOptions || {});
	}
	TimeoutTask(requestData: IGetTaskRequest, errorOptions?: IActionErrorOptions) {
		return this.TimeoutTaskAction.execute(requestData, errorOptions || {});
	}
}