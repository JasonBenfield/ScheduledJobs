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
		this.DeleteJobsWithNoTasksAction = this.createAction<IDeleteJobsWithNoTasksRequest,IEmptyActionResult>('DeleteJobsWithNoTasks', 'Delete Jobs With No Tasks');
		this.RetryJobsAction = this.createAction<IRetryJobsRequest,ITriggeredJobWithTasksModel[]>('RetryJobs', 'Retry Jobs');
		this.StartJobAction = this.createAction<IStartJobRequest,ITriggeredJobWithTasksModel>('StartJob', 'Start Job');
		this.StartTaskAction = this.createAction<IStartTaskRequest,IEmptyActionResult>('StartTask', 'Start Task');
		this.JobCancelledAction = this.createAction<IJobCancelledRequest,IEmptyActionResult>('JobCancelled', 'Job Cancelled');
		this.TaskCompletedAction = this.createAction<ITaskCompletedRequest,ITriggeredJobWithTasksModel>('TaskCompleted', 'Task Completed');
		this.TaskFailedAction = this.createAction<ITaskFailedRequest,ITriggeredJobWithTasksModel>('TaskFailed', 'Task Failed');
		this.LogMessageAction = this.createAction<ILogMessageRequest,IEmptyActionResult>('LogMessage', 'Log Message');
	}
	
	readonly AddOrUpdateRegisteredJobsAction: AppApiAction<IRegisteredJob[],IEmptyActionResult>;
	readonly TriggerJobsAction: AppApiAction<ITriggerJobsRequest,IPendingJobModel[]>;
	readonly DeleteJobsWithNoTasksAction: AppApiAction<IDeleteJobsWithNoTasksRequest,IEmptyActionResult>;
	readonly RetryJobsAction: AppApiAction<IRetryJobsRequest,ITriggeredJobWithTasksModel[]>;
	readonly StartJobAction: AppApiAction<IStartJobRequest,ITriggeredJobWithTasksModel>;
	readonly StartTaskAction: AppApiAction<IStartTaskRequest,IEmptyActionResult>;
	readonly JobCancelledAction: AppApiAction<IJobCancelledRequest,IEmptyActionResult>;
	readonly TaskCompletedAction: AppApiAction<ITaskCompletedRequest,ITriggeredJobWithTasksModel>;
	readonly TaskFailedAction: AppApiAction<ITaskFailedRequest,ITriggeredJobWithTasksModel>;
	readonly LogMessageAction: AppApiAction<ILogMessageRequest,IEmptyActionResult>;
	
	AddOrUpdateRegisteredJobs(model: IRegisteredJob[], errorOptions?: IActionErrorOptions) {
		return this.AddOrUpdateRegisteredJobsAction.execute(model, errorOptions || {});
	}
	TriggerJobs(model: ITriggerJobsRequest, errorOptions?: IActionErrorOptions) {
		return this.TriggerJobsAction.execute(model, errorOptions || {});
	}
	DeleteJobsWithNoTasks(model: IDeleteJobsWithNoTasksRequest, errorOptions?: IActionErrorOptions) {
		return this.DeleteJobsWithNoTasksAction.execute(model, errorOptions || {});
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
	JobCancelled(model: IJobCancelledRequest, errorOptions?: IActionErrorOptions) {
		return this.JobCancelledAction.execute(model, errorOptions || {});
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