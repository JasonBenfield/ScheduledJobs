// Generated code

import { AppClientGroup } from "@jasonbenfield/sharedwebapp/Http/AppClientGroup";
import { AppClientAction } from "@jasonbenfield/sharedwebapp/Http/AppClientAction";
import { AppClientView } from "@jasonbenfield/sharedwebapp/Http/AppClientView";
import { AppClientEvents } from "@jasonbenfield/sharedwebapp/Http/AppClientEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Http/AppResourceUrl";

export class EventDefinitionsGroup extends AppClientGroup {
	constructor(events: AppClientEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'EventDefinitions');
		this.Index = this.createView<IEmptyRequest>('Index');
		this.GetEventDefinitionsAction = this.createAction<IEmptyRequest,IEventDefinitionModel[]>('GetEventDefinitions', 'Get Event Definitions');
		this.GetRecentNotificationsAction = this.createAction<IGetRecentEventNotificationsByEventDefinitionRequest,IEventSummaryModel[]>('GetRecentNotifications', 'Get Recent Notifications');
	}
	
	readonly Index: AppClientView<IEmptyRequest>;
	readonly GetEventDefinitionsAction: AppClientAction<IEmptyRequest,IEventDefinitionModel[]>;
	readonly GetRecentNotificationsAction: AppClientAction<IGetRecentEventNotificationsByEventDefinitionRequest,IEventSummaryModel[]>;
	
	GetEventDefinitions(errorOptions?: IActionErrorOptions) {
		return this.GetEventDefinitionsAction.execute({}, errorOptions || {});
	}
	GetRecentNotifications(model: IGetRecentEventNotificationsByEventDefinitionRequest, errorOptions?: IActionErrorOptions) {
		return this.GetRecentNotificationsAction.execute(model, errorOptions || {});
	}
}