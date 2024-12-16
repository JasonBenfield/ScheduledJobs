// Generated code

import * as xti from "@jasonbenfield/sharedwebapp/Common";
import { AppClientGroup } from "@jasonbenfield/sharedwebapp/Http/AppClientGroup";
import { AppClientAction } from "@jasonbenfield/sharedwebapp/Http/AppClientAction";
import { AppClientView } from "@jasonbenfield/sharedwebapp/Http/AppClientView";
import { AppClientEvents } from "@jasonbenfield/sharedwebapp/Http/AppClientEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Http/AppResourceUrl";

export class JobDefinitionsGroup extends AppClientGroup {
	constructor(events: AppClientEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'JobDefinitions');
		this.GetJobDefinitionsAction = this.createAction<IEmptyRequest,IJobDefinitionModel[]>('GetJobDefinitions', 'Get Job Definitions');
		this.GetRecentTriggeredJobsAction = this.createAction<IGetRecentTriggeredJobsByDefinitionRequest,IJobSummaryModel[]>('GetRecentTriggeredJobs', 'Get Recent Triggered Jobs');
		this.Index = this.createView<IEmptyRequest>('Index');
	}
	
	readonly GetJobDefinitionsAction: AppClientAction<IEmptyRequest,IJobDefinitionModel[]>;
	readonly GetRecentTriggeredJobsAction: AppClientAction<IGetRecentTriggeredJobsByDefinitionRequest,IJobSummaryModel[]>;
	readonly Index: AppClientView<IEmptyRequest>;
	
	GetJobDefinitions(errorOptions?: IActionErrorOptions) {
		return this.GetJobDefinitionsAction.execute({}, errorOptions || {});
	}
	GetRecentTriggeredJobs(model: IGetRecentTriggeredJobsByDefinitionRequest, errorOptions?: IActionErrorOptions) {
		return this.GetRecentTriggeredJobsAction.execute(model, errorOptions || {});
	}
}