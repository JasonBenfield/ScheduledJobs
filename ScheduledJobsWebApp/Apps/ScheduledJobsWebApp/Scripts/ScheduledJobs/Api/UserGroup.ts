// Generated code

import { AppApiGroup } from "@jasonbenfield/sharedwebapp/Api/AppApiGroup";
import { AppApiAction } from "@jasonbenfield/sharedwebapp/Api/AppApiAction";
import { AppApiView } from "@jasonbenfield/sharedwebapp/Api/AppApiView";
import { AppApiEvents } from "@jasonbenfield/sharedwebapp/Api/AppApiEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Api/AppResourceUrl";

export class UserGroup extends AppApiGroup implements IUserGroup {
	constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'User');
		this.Index = this.createView<IUserStartRequest>('Index');
		this.AccessDenied = this.createView<IEmptyRequest>('AccessDenied');
		this.Error = this.createView<IEmptyRequest>('Error');
		this.Logout = this.createView<ILogoutRequest>('Logout');
	}
	
	readonly Index: AppApiView<IUserStartRequest>;
	readonly AccessDenied: AppApiView<IEmptyRequest>;
	readonly Error: AppApiView<IEmptyRequest>;
	readonly Logout: AppApiView<ILogoutRequest>;
	
}