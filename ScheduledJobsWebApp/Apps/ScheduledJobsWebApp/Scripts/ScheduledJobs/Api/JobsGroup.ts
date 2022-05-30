// Generated code

import { AppApiGroup } from "@jasonbenfield/sharedwebapp/Api/AppApiGroup";
import { AppApiAction } from "@jasonbenfield/sharedwebapp/Api/AppApiAction";
import { AppApiView } from "@jasonbenfield/sharedwebapp/Api/AppApiView";
import { AppApiEvents } from "@jasonbenfield/sharedwebapp/Api/AppApiEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Api/AppResourceUrl";

export class JobsGroup extends AppApiGroup {
	constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'Jobs');
		this.AddOrUpdateRegisteredJobsAction = this.createAction<IRegisteredJob[],IEmptyActionResult>('AddOrUpdateRegisteredJobs', 'Add Or Update Registered Jobs');
		this.TriggerJobsAction = this.createAction<ITriggerJobsRequest,IPendingJobModel[]>('TriggerJobs', 'Trigger Jobs');
		this.RetryJobsAction = this.createAction<IRetryJobsRequest,ITriggeredJobDetailModel[]>('RetryJobs', 'Retry Jobs');
		this.StartJobAction = this.createAction<IStartJobRequest,ITriggeredJobDetailModel>('StartJob', 'Start Job');
		this.StartTaskAction = this.createAction<IStartTaskRequest,IEmptyActionResult>('StartTask', 'Start Task');
		this.TaskCompletedAction = this.createAction<ITaskCompletedRequest,ITriggeredJobDetailModel>('TaskCompleted', 'Task Completed');
		this.TaskFailedAction = this.createAction<ITaskFailedRequest,ITriggeredJobDetailModel>('TaskFailed', 'Task Failed');
		this.LogMessageAction = this.createAction<ILogMessageRequest,IEmptyActionResult>('LogMessage', 'Log Message');
	}
	
	readonly AddOrUpdateRegisteredJobsAction: AppApiAction<IRegisteredJob[],IEmptyActionResult>;
	readonly TriggerJobsAction: AppApiAction<ITriggerJobsRequest,IPendingJobModel[]>;
	readonly RetryJobsAction: AppApiAction<IRetryJobsRequest,ITriggeredJobDetailModel[]>;
	readonly StartJobAction: AppApiAction<IStartJobRequest,ITriggeredJobDetailModel>;
	readonly StartTaskAction: AppApiAction<IStartTaskRequest,IEmptyActionResult>;
	readonly TaskCompletedAction: AppApiAction<ITaskCompletedRequest,ITriggeredJobDetailModel>;
	readonly TaskFailedAction: AppApiAction<ITaskFailedRequest,ITriggeredJobDetailModel>;
	readonly LogMessageAction: AppApiAction<ILogMessageRequest,IEmptyActionResult>;
	
	AddOrUpdateRegisteredJobs(model: IRegisteredJob[], errorOptions?: IActionErrorOptions) {
		return this.AddOrUpdateRegisteredJobsAction.execute(model, errorOptions || {});
	}
	TriggerJobs(model: ITriggerJobsRequest, errorOptions?: IActionErrorOptions) {
		return this.TriggerJobsAction.execute(model, errorOptions || {});
	}
	RetryJobs(model: IRetryJobsRequest, errorOptions?: IActionErrorOptions) {
		return this.RetryJobsAction.execute(model, errorOptions || {});
	}
	StartJob(model: IStartJobRequest, errorOptions?: IActionErrorOptions) {
		return this.StartJobAction.execute(model, errorOptions || {});
	}
	StartTask(model: IStartTaskRequest, errorOptions?: IActionErrorOptions) {
		return this.StartTaskAction.execute(model, errorOptions || {});
	}
	TaskCompleted(model: ITaskCompletedRequest, errorOptions?: IActionErrorOptions) {
		return this.TaskCompletedAction.execute(model, errorOptions || {});
	}
	TaskFailed(model: ITaskFailedRequest, errorOptions?: IActionErrorOptions) {
		return this.TaskFailedAction.execute(model, errorOptions || {});
	}
	LogMessage(model: ILogMessageRequest, errorOptions?: IActionErrorOptions) {
		return this.LogMessageAction.execute(model, errorOptions || {});
	}
}