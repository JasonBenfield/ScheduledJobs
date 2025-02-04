// Generated code

import * as xti from "@jasonbenfield/sharedwebapp/Common";
import { AppClientGroup } from "@jasonbenfield/sharedwebapp/Http/AppClientGroup";
import { AppClientAction } from "@jasonbenfield/sharedwebapp/Http/AppClientAction";
import { AppClientView } from "@jasonbenfield/sharedwebapp/Http/AppClientView";
import { AppClientEvents } from "@jasonbenfield/sharedwebapp/Http/AppClientEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Http/AppResourceUrl";

export class EventDefinitionsGroup extends AppClientGroup {
	constructor(events: AppClientEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'EventDefinitions');
		this.GetEventDefinitionsAction = this.createAction<IEmptyRequest,IEventDefinitionModel[]>('GetEventDefinitions', 'Get Event Definitions');
		this.GetRecentNotificationsAction = this.createAction<IGetRecentEventNotificationsByEventDefinitionRequest,IEventSummaryModel[]>('GetRecentNotifications', 'Get Recent Notifications');
		this.Index = this.createView<IEmptyRequest>('Index');
	}
	
	readonly GetEventDefinitionsAction: AppClientAction<IEmptyRequest,IEventDefinitionModel[]>;
	readonly GetRecentNotificationsAction: AppClientAction<IGetRecentEventNotificationsByEventDefinitionRequest,IEventSummaryModel[]>;
	readonly Index: AppClientView<IEmptyRequest>;
	
	GetEventDefinitions(errorOptions?: IActionErrorOptions) {
		return this.GetEventDefinitionsAction.execute({}, errorOptions || {});
	}
	GetRecentNotifications(requestData: IGetRecentEventNotificationsByEventDefinitionRequest, errorOptions?: IActionErrorOptions) {
		return this.GetRecentNotificationsAction.execute(requestData, errorOptions || {});
	}
}