// Generated code

interface ILinkModel {
	LinkName: string;
	DisplayText: string;
	Url: string;
	IsAuthenticationRequired: boolean;
}
interface IEventDefinitionModel {
	ID: number;
	EventKey: IEventKey;
}
interface IEventKey {
	Value: string;
	DisplayText: string;
}
interface IGetRecentEventNotificationsByEventDefinitionRequest {
	EventDefinitionID: number;
	SourceKey: string;
}
interface IEventSummaryModel {
	Event: IEventNotificationModel;
	TriggeredJobCount: number;
}
interface IEventNotificationModel {
	ID: number;
	Definition: IEventDefinitionModel;
	SourceKey: string;
	SourceData: string;
	TimeAdded: import('@jasonbenfield/sharedwebapp/Common').DateTimeOffset;
	TimeActive: import('@jasonbenfield/sharedwebapp/Common').DateTimeOffset;
	TimeInactive: import('@jasonbenfield/sharedwebapp/Common').DateTimeOffset;
}
interface IGetNotificationDetailRequest {
	NotificationID: number;
}
interface IEventNotificationDetailModel {
	Event: IEventNotificationModel;
	TriggeredJobs: IJobSummaryModel[];
}
interface IJobSummaryModel {
	ID: number;
	JobKey: IJobKey;
	Status: IJobTaskStatus;
	TimeStarted: import('@jasonbenfield/sharedwebapp/Common').DateTimeOffset;
	TimeEnded: import('@jasonbenfield/sharedwebapp/Common').DateTimeOffset;
	TaskCount: number;
}
interface IJobKey {
	Value: string;
	DisplayText: string;
}
interface IRegisteredEvent {
	EventKey: IEventKey;
	CompareSourceKeyAndDataForDuplication: boolean;
	DuplicateHandling: IDuplicateHandling;
	TimeToStartNotifications: import('@jasonbenfield/sharedwebapp/Common').DateTimeOffset;
	ActiveFor: import('@jasonbenfield/sharedwebapp/Common').TimeSpan;
	DeleteAfter: import('@jasonbenfield/sharedwebapp/Common').TimeSpan;
}
interface IAddNotificationsRequest {
	EventKey: string;
	Sources: IXtiEventSource[];
}
interface IXtiEventSource {
	SourceKey: string;
	SourceData: string;
}
interface ITriggeredJobsRequest {
	EventNotificationID: number;
}
interface ITriggeredJobWithTasksModel {
	Job: ITriggeredJobModel;
	Tasks: ITriggeredJobTaskModel[];
}
interface ITriggeredJobModel {
	ID: number;
	JobDefinition: IJobDefinitionModel;
	EventNotificationID: number;
}
interface IJobDefinitionModel {
	ID: number;
	JobKey: IJobKey;
}
interface ITriggeredJobTaskModel {
	ID: number;
	TaskDefinition: IJobTaskDefinitionModel;
	Status: IJobTaskStatus;
	Generation: number;
	TimeStarted: import('@jasonbenfield/sharedwebapp/Common').DateTimeOffset;
	TimeEnded: import('@jasonbenfield/sharedwebapp/Common').DateTimeOffset;
	TaskData: string;
	LogEntries: IJobLogEntryModel[];
}
interface IJobTaskDefinitionModel {
	ID: number;
	TaskKey: IJobTaskKey;
}
interface IJobTaskKey {
	Value: string;
	DisplayText: string;
}
interface IJobLogEntryModel {
	ID: number;
	Severity: IAppEventSeverity;
	TimeOccurred: import('@jasonbenfield/sharedwebapp/Common').DateTimeOffset;
	Category: string;
	Message: string;
	Details: string;
	SourceEventKey: string;
}
interface IGetRecentTriggeredJobsByDefinitionRequest {
	JobDefinitionID: number;
}
interface IGetJobDetailRequest {
	JobID: number;
}
interface ITriggeredJobDetailModel {
	Job: ITriggeredJobModel;
	TriggeredBy: IEventNotificationModel;
	Tasks: ITriggeredJobTaskModel[];
	SourceLogEntries: ISourceLogEntryModel[];
}
interface ISourceLogEntryModel {
	LogEntryID: number;
	SourceLogEntry: IAppLogEntryModel;
}
interface IAppLogEntryModel {
	ID: number;
	RequestID: number;
	TimeOccurred: import('@jasonbenfield/sharedwebapp/Common').DateTimeOffset;
	Severity: IAppEventSeverity;
	Caption: string;
	Message: string;
	Detail: string;
	Category: string;
	ActualCount: number;
}
interface IAddOrUpdateJobSchedulesRequest {
	JobKey: string;
	Schedules: string;
	DeleteAfter: import('@jasonbenfield/sharedwebapp/Common').TimeSpan;
}
interface IRegisteredJob {
	JobKey: IJobKey;
	Timeout: import('@jasonbenfield/sharedwebapp/Common').TimeSpan;
	DeleteAfter: import('@jasonbenfield/sharedwebapp/Common').TimeSpan;
	Tasks: IRegisteredJobTask[];
}
interface IRegisteredJobTask {
	TaskKey: IJobTaskKey;
	Timeout: import('@jasonbenfield/sharedwebapp/Common').TimeSpan;
}
interface ITriggerJobsRequest {
	EventKey: string;
	JobKey: string;
	EventRaisedStartTime: import('@jasonbenfield/sharedwebapp/Common').DateTimeOffset;
}
interface IPendingJobModel {
	Job: ITriggeredJobModel;
	SourceKey: string;
	SourceData: string;
}
interface IDeleteJobsWithNoTasksRequest {
	EventKey: string;
	JobKey: string;
}
interface IRetryJobsRequest {
	EventKey: string;
	JobKey: string;
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
interface IJobCancelledRequest {
	TaskID: number;
	Reason: string;
}
interface ITaskCompletedRequest {
	CompletedTaskID: number;
	PreserveData: boolean;
	NextTasks: INextTaskModel[];
}
interface ITaskFailedRequest {
	FailedTaskID: number;
	ErrorStatus: number;
	RetryAfter: import('@jasonbenfield/sharedwebapp/Common').TimeSpan;
	NextTasks: INextTaskModel[];
	Category: string;
	Message: string;
	Detail: string;
	SourceLogEntryKey: string;
}
interface ILogMessageRequest {
	TaskID: number;
	Category: string;
	Message: string;
	Details: string;
}
interface IGetTaskRequest {
	TaskID: number;
}
interface IEditTaskDataRequest {
	TaskID: number;
	TaskData: string;
}
interface IJobTaskStatus {
	Value: number;
	DisplayText: string;
}
interface IDuplicateHandling {
	Value: number;
	DisplayText: string;
}
interface IAppEventSeverity {
	Value: number;
	DisplayText: string;
}