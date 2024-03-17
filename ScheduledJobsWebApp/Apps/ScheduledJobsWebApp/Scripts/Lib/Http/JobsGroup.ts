// Generated code

import * as xti from "@jasonbenfield/sharedwebapp/Common";
import { AppClientGroup } from "@jasonbenfield/sharedwebapp/Http/AppClientGroup";
import { AppClientAction } from "@jasonbenfield/sharedwebapp/Http/AppClientAction";
import { AppClientView } from "@jasonbenfield/sharedwebapp/Http/AppClientView";
import { AppClientEvents } from "@jasonbenfield/sharedwebapp/Http/AppClientEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Http/AppResourceUrl";

export class JobsGroup extends AppClientGroup {
	constructor(events: AppClientEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'Jobs');
		this.AddOrUpdateJobSchedulesAction = this.createAction<IAddOrUpdateJobSchedulesRequest,IEmptyActionResult>('AddOrUpdateJobSchedules', 'Add Or Update Job Schedules');
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
	
	readonly AddOrUpdateJobSchedulesAction: AppClientAction<IAddOrUpdateJobSchedulesRequest,IEmptyActionResult>;
	readonly AddOrUpdateRegisteredJobsAction: AppClientAction<IRegisteredJob[],IEmptyActionResult>;
	readonly TriggerJobsAction: AppClientAction<ITriggerJobsRequest,IPendingJobModel[]>;
	readonly DeleteJobsWithNoTasksAction: AppClientAction<IDeleteJobsWithNoTasksRequest,IEmptyActionResult>;
	readonly RetryJobsAction: AppClientAction<IRetryJobsRequest,ITriggeredJobWithTasksModel[]>;
	readonly StartJobAction: AppClientAction<IStartJobRequest,ITriggeredJobWithTasksModel>;
	readonly StartTaskAction: AppClientAction<IStartTaskRequest,IEmptyActionResult>;
	readonly JobCancelledAction: AppClientAction<IJobCancelledRequest,IEmptyActionResult>;
	readonly TaskCompletedAction: AppClientAction<ITaskCompletedRequest,ITriggeredJobWithTasksModel>;
	readonly TaskFailedAction: AppClientAction<ITaskFailedRequest,ITriggeredJobWithTasksModel>;
	readonly LogMessageAction: AppClientAction<ILogMessageRequest,IEmptyActionResult>;
	
	AddOrUpdateJobSchedules(model: IAddOrUpdateJobSchedulesRequest, errorOptions?: IActionErrorOptions) {
		return this.AddOrUpdateJobSchedulesAction.execute(model, errorOptions || {});
	}
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