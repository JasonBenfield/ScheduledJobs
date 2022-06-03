// Generated code

interface IUserStartRequest {
	ReturnUrl: string;
}
interface IEmptyRequest {
}
interface ILogoutRequest {
	ReturnUrl: string;
}
interface IEmptyActionResult {
}
interface IRegisteredEvent {
	EventKey: IEventKey;
	CompareSourceKeyAndDataForDuplication: boolean;
	DuplicateHandling: IDuplicateHandling;
	TimeToStartNotifications: Date;
	ActiveFor: string;
}
interface IEventKey {
	Value: string;
	DisplayText: string;
}
interface IAddNotificationsRequest {
	EventKey: IEventKey;
	Sources: IEventSource[];
}
interface IEventSource {
	SourceKey: string;
	SourceData: string;
}
interface IEventNotificationModel {
	ID: number;
}
interface ITriggeredJobsRequest {
	EventNotificationID: number;
}
interface ITriggeredJobDetailModel {
	Job: ITriggeredJobModel;
	Tasks: ITriggeredJobTaskModel[];
}
interface ITriggeredJobModel {
	ID: number;
	JobDefinition: IJobDefinitionModel;
}
interface IJobDefinitionModel {
	ID: number;
	JobKey: IJobKey;
}
interface IJobKey {
	Value: string;
	DisplayText: string;
}
interface ITriggeredJobTaskModel {
	ID: number;
	TaskDefinition: IJobTaskDefinitionModel;
	Status: IJobTaskStatus;
	TaskData: string;
	LogEntries: ILogEntryModel[];
}
interface IJobTaskDefinitionModel {
	ID: number;
	TaskKey: IJobTaskKey;
}
interface IJobTaskKey {
	Value: string;
	DisplayText: string;
}
interface ILogEntryModel {
	ID: number;
	Severity: IAppEventSeverity;
	Category: string;
	Message: string;
	Details: string;
}
interface IRegisteredJob {
	JobKey: IJobKey;
	Timeout: string;
	Tasks: IRegisteredJobTask[];
}
interface IRegisteredJobTask {
	TaskKey: IJobTaskKey;
	Timeout: string;
}
interface ITriggerJobsRequest {
	EventKey: IEventKey;
	JobKey: IJobKey;
	EventRaisedStartTime: Date;
}
interface IPendingJobModel {
	Job: ITriggeredJobModel;
	SourceData: string;
}
interface IRetryJobsRequest {
	JobKey: IJobKey;
}
interface IStartJobRequest {
	JobID: number;
	NextTasks: INextTaskModel[];
}
interface INextTaskModel {
	TaskKey: IJobTaskKey;
	TaskData: string;
}
interface IStartTaskRequest {
	TaskID: number;
}
interface ITaskCompletedRequest {
	JobID: number;
	CompletedTaskID: number;
	PreserveData: boolean;
	NextTasks: INextTaskModel[];
}
interface ITaskFailedRequest {
	JobID: number;
	FailedTaskID: number;
	ErrorStatus: IJobTaskStatus;
	RetryAfter: string;
	NextTasks: INextTaskModel[];
	Category: string;
	Message: string;
	Detail: string;
}
interface ILogMessageRequest {
	TaskID: number;
	Category: string;
	Message: string;
	Details: string;
}
interface IDuplicateHandling {
	Value: number;
	DisplayText: string;
}
interface IJobTaskStatus {
	Value: number;
	DisplayText: string;
}
interface IAppEventSeverity {
	Value: number;
	DisplayText: string;
}