// Generated code

import { AppApiGroup } from "@jasonbenfield/sharedwebapp/Api/AppApiGroup";
import { AppApiAction } from "@jasonbenfield/sharedwebapp/Api/AppApiAction";
import { AppApiView } from "@jasonbenfield/sharedwebapp/Api/AppApiView";
import { AppApiEvents } from "@jasonbenfield/sharedwebapp/Api/AppApiEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Api/AppResourceUrl";

export class JobInquiryGroup extends AppApiGroup {
	constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'JobInquiry');
		this.FailedJobs = this.createView<IEmptyRequest>('FailedJobs');
		this.GetFailedJobsAction = this.createAction<IEmptyRequest,IJobSummaryModel[]>('GetFailedJobs', 'Get Failed Jobs');
	}
	
	readonly FailedJobs: AppApiView<IEmptyRequest>;
	readonly GetFailedJobsAction: AppApiAction<IEmptyRequest,IJobSummaryModel[]>;
	
	GetFailedJobs(errorOptions?: IActionErrorOptions) {
		return this.GetFailedJobsAction.execute({}, errorOptions || {});
	}
}