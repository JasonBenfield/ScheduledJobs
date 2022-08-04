// Generated code

import { AppApiGroup } from "@jasonbenfield/sharedwebapp/Api/AppApiGroup";
import { AppApiAction } from "@jasonbenfield/sharedwebapp/Api/AppApiAction";
import { AppApiView } from "@jasonbenfield/sharedwebapp/Api/AppApiView";
import { AppApiEvents } from "@jasonbenfield/sharedwebapp/Api/AppApiEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Api/AppResourceUrl";

export class UserCacheGroup extends AppApiGroup {
	constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'UserCache');
		this.ClearCacheAction = this.createAction<string,IEmptyActionResult>('ClearCache', 'Clear Cache');
	}
	
	readonly ClearCacheAction: AppApiAction<string,IEmptyActionResult>;
	
	ClearCache(model: string, errorOptions?: IActionErrorOptions) {
		return this.ClearCacheAction.execute(model, errorOptions || {});
	}
}