// Generated code

interface IResourcePath {
	Group: string;
	Action: string;
	ModKey: string;
}
interface IResourcePathAccess {
	Path: IResourcePath;
	HasAccess: boolean;
}
interface IEmptyRequest {
}
interface ILogoutRequest {
	ReturnUrl: string;
}
interface IEmptyActionResult {
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
	TimeAdded: Date;
	TimeActive: Date;
	TimeInactive: Date;
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
	TimeStarted: Date;
	TimeEnded: Date;
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
	TimeToStartNotifications: Date;
	ActiveFor: string;
	DeleteAfter: string;
}
interface IAddNotificationsRequest {
	EventKey: IEventKey;
	Sources: IEventSource[];
}
interface IEventSource {
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
	TimeStarted: Date;
	TimeEnded: Date;
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
	TimeOccurred: Date;
	Category: string;
	Message: string;
	Details: string;
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
}
interface IRegisteredJob {
	JobKey: IJobKey;
	Timeout: string;
	DeleteAfter: string;
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
interface IJobCancelledRequest {
	TaskID: number;
	Reason: string;
	DeletionTime: IDeletionTime;
}
interface ITaskCompletedRequest {
	CompletedTaskID: number;
	PreserveData: boolean;
	NextTasks: INextTaskModel[];
}
interface ITaskFailedRequest {
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
interface IGetTaskRequest {
	TaskID: number;
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
interface IDeletionTime {
	Value: number;
	DisplayText: string;
}