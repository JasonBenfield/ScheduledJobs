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
		this.DeleteJobsWithNoTasksAction = this.createAction<IDeleteJobsWithNoTasksRequest,IEmptyActionResult>('DeleteJobsWithNoTasks', 'Delete Jobs With No Tasks');
		this.JobCancelledAction = this.createAction<IJobCancelledRequest,IEmptyActionResult>('JobCancelled', 'Job Cancelled');
		this.LogMessageAction = this.createAction<ILogMessageRequest,IEmptyActionResult>('LogMessage', 'Log Message');
		this.RetryJobsAction = this.createAction<IRetryJobsRequest,ITriggeredJobWithTasksModel[]>('RetryJobs', 'Retry Jobs');
		this.StartJobAction = this.createAction<IStartJobRequest,ITriggeredJobWithTasksModel>('StartJob', 'Start Job');
		this.StartTaskAction = this.createAction<IStartTaskRequest,IEmptyActionResult>('StartTask', 'Start Task');
		this.TaskCompletedAction = this.createAction<ITaskCompletedRequest,ITriggeredJobWithTasksModel>('TaskCompleted', 'Task Completed');
		this.TaskFailedAction = this.createAction<ITaskFailedRequest,ITriggeredJobWithTasksModel>('TaskFailed', 'Task Failed');
		this.TriggerJobsAction = this.createAction<ITriggerJobsRequest,IPendingJobModel[]>('TriggerJobs', 'Trigger Jobs');
	}
	
	readonly AddOrUpdateJobSchedulesAction: AppClientAction<IAddOrUpdateJobSchedulesRequest,IEmptyActionResult>;
	readonly AddOrUpdateRegisteredJobsAction: AppClientAction<IRegisteredJob[],IEmptyActionResult>;
	readonly DeleteJobsWithNoTasksAction: AppClientAction<IDeleteJobsWithNoTasksRequest,IEmptyActionResult>;
	readonly JobCancelledAction: AppClientAction<IJobCancelledRequest,IEmptyActionResult>;
	readonly LogMessageAction: AppClientAction<ILogMessageRequest,IEmptyActionResult>;
	readonly RetryJobsAction: AppClientAction<IRetryJobsRequest,ITriggeredJobWithTasksModel[]>;
	readonly StartJobAction: AppClientAction<IStartJobRequest,ITriggeredJobWithTasksModel>;
	readonly StartTaskAction: AppClientAction<IStartTaskRequest,IEmptyActionResult>;
	readonly TaskCompletedAction: AppClientAction<ITaskCompletedRequest,ITriggeredJobWithTasksModel>;
	readonly TaskFailedAction: AppClientAction<ITaskFailedRequest,ITriggeredJobWithTasksModel>;
	readonly TriggerJobsAction: AppClientAction<ITriggerJobsRequest,IPendingJobModel[]>;
	
	AddOrUpdateJobSchedules(requestData: IAddOrUpdateJobSchedulesRequest, errorOptions?: IActionErrorOptions) {
		return this.AddOrUpdateJobSchedulesAction.execute(requestData, errorOptions || {});
	}
	AddOrUpdateRegisteredJobs(requestData: IRegisteredJob[], errorOptions?: IActionErrorOptions) {
		return this.AddOrUpdateRegisteredJobsAction.execute(requestData, errorOptions || {});
	}
	DeleteJobsWithNoTasks(requestData: IDeleteJobsWithNoTasksRequest, errorOptions?: IActionErrorOptions) {
		return this.DeleteJobsWithNoTasksAction.execute(requestData, errorOptions || {});
	}
	JobCancelled(requestData: IJobCancelledRequest, errorOptions?: IActionErrorOptions) {
		return this.JobCancelledAction.execute(requestData, errorOptions || {});
	}
	LogMessage(requestData: ILogMessageRequest, errorOptions?: IActionErrorOptions) {
		return this.LogMessageAction.execute(requestData, errorOptions || {});
	}
	RetryJobs(requestData: IRetryJobsRequest, errorOptions?: IActionErrorOptions) {
		return this.RetryJobsAction.execute(requestData, errorOptions || {});
	}
	StartJob(requestData: IStartJobRequest, errorOptions?: IActionErrorOptions) {
		return this.StartJobAction.execute(requestData, errorOptions || {});
	}
	StartTask(requestData: IStartTaskRequest, errorOptions?: IActionErrorOptions) {
		return this.StartTaskAction.execute(requestData, errorOptions || {});
	}
	TaskCompleted(requestData: ITaskCompletedRequest, errorOptions?: IActionErrorOptions) {
		return this.TaskCompletedAction.execute(requestData, errorOptions || {});
	}
	TaskFailed(requestData: ITaskFailedRequest, errorOptions?: IActionErrorOptions) {
		return this.TaskFailedAction.execute(requestData, errorOptions || {});
	}
	TriggerJobs(requestData: ITriggerJobsRequest, errorOptions?: IActionErrorOptions) {
		return this.TriggerJobsAction.execute(requestData, errorOptions || {});
	}
}