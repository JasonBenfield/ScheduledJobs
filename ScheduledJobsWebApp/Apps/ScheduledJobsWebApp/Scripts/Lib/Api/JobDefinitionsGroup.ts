// Generated code

import { AppApiGroup } from "@jasonbenfield/sharedwebapp/Api/AppApiGroup";
import { AppApiAction } from "@jasonbenfield/sharedwebapp/Api/AppApiAction";
import { AppApiView } from "@jasonbenfield/sharedwebapp/Api/AppApiView";
import { AppApiEvents } from "@jasonbenfield/sharedwebapp/Api/AppApiEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Api/AppResourceUrl";

export class JobDefinitionsGroup extends AppApiGroup {
	constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'JobDefinitions');
		this.Index = this.createView<IEmptyRequest>('Index');
		this.GetJobDefinitionsAction = this.createAction<IEmptyRequest,IJobDefinitionModel[]>('GetJobDefinitions', 'Get Job Definitions');
		this.GetRecentTriggeredJobsAction = this.createAction<IGetRecentTriggeredJobsByDefinitionRequest,IJobSummaryModel[]>('GetRecentTriggeredJobs', 'Get Recent Triggered Jobs');
	}
	
	readonly Index: AppApiView<IEmptyRequest>;
	readonly GetJobDefinitionsAction: AppApiAction<IEmptyRequest,IJobDefinitionModel[]>;
	readonly GetRecentTriggeredJobsAction: AppApiAction<IGetRecentTriggeredJobsByDefinitionRequest,IJobSummaryModel[]>;
	
	GetJobDefinitions(errorOptions?: IActionErrorOptions) {
		return this.GetJobDefinitionsAction.execute({}, errorOptions || {});
	}
	GetRecentTriggeredJobs(model: IGetRecentTriggeredJobsByDefinitionRequest, errorOptions?: IActionErrorOptions) {
		return this.GetRecentTriggeredJobsAction.execute(model, errorOptions || {});
	}
}