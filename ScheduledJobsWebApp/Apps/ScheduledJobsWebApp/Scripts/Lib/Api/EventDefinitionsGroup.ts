// Generated code

import { AppApiGroup } from "@jasonbenfield/sharedwebapp/Api/AppApiGroup";
import { AppApiAction } from "@jasonbenfield/sharedwebapp/Api/AppApiAction";
import { AppApiView } from "@jasonbenfield/sharedwebapp/Api/AppApiView";
import { AppApiEvents } from "@jasonbenfield/sharedwebapp/Api/AppApiEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Api/AppResourceUrl";

export class EventDefinitionsGroup extends AppApiGroup {
	constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'EventDefinitions');
		this.Index = this.createView<IEmptyRequest>('Index');
		this.GetEventDefinitionsAction = this.createAction<IEmptyRequest,IEventDefinitionModel[]>('GetEventDefinitions', 'Get Event Definitions');
		this.GetRecentNotificationsAction = this.createAction<IGetRecentEventNotificationsByEventDefinitionRequest,IEventSummaryModel[]>('GetRecentNotifications', 'Get Recent Notifications');
	}
	
	readonly Index: AppApiView<IEmptyRequest>;
	readonly GetEventDefinitionsAction: AppApiAction<IEmptyRequest,IEventDefinitionModel[]>;
	readonly GetRecentNotificationsAction: AppApiAction<IGetRecentEventNotificationsByEventDefinitionRequest,IEventSummaryModel[]>;
	
	GetEventDefinitions(errorOptions?: IActionErrorOptions) {
		return this.GetEventDefinitionsAction.execute({}, errorOptions || {});
	}
	GetRecentNotifications(model: IGetRecentEventNotificationsByEventDefinitionRequest, errorOptions?: IActionErrorOptions) {
		return this.GetRecentNotificationsAction.execute(model, errorOptions || {});
	}
}