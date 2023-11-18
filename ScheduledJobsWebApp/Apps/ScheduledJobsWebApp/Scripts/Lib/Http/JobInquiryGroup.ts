// Generated code

import { AppClientGroup } from "@jasonbenfield/sharedwebapp/Http/AppClientGroup";
import { AppClientAction } from "@jasonbenfield/sharedwebapp/Http/AppClientAction";
import { AppClientView } from "@jasonbenfield/sharedwebapp/Http/AppClientView";
import { AppClientEvents } from "@jasonbenfield/sharedwebapp/Http/AppClientEvents";
import { AppResourceUrl } from "@jasonbenfield/sharedwebapp/Http/AppResourceUrl";

export class JobInquiryGroup extends AppClientGroup {
	constructor(events: AppClientEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'JobInquiry');
		this.FailedJobs = this.createView<IEmptyRequest>('FailedJobs');
		this.GetFailedJobsAction = this.createAction<IEmptyRequest,IJobSummaryModel[]>('GetFailedJobs', 'Get Failed Jobs');
		this.RecentJobs = this.createView<IEmptyRequest>('RecentJobs');
		this.GetRecentJobsAction = this.createAction<IEmptyRequest,IJobSummaryModel[]>('GetRecentJobs', 'Get Recent Jobs');
		this.JobDetail = this.createView<IGetJobDetailRequest>('JobDetail');
		this.GetJobDetailAction = this.createAction<IGetJobDetailRequest,ITriggeredJobDetailModel>('GetJobDetail', 'Get Job Detail');
	}
	
	readonly FailedJobs: AppClientView<IEmptyRequest>;
	readonly GetFailedJobsAction: AppClientAction<IEmptyRequest,IJobSummaryModel[]>;
	readonly RecentJobs: AppClientView<IEmptyRequest>;
	readonly GetRecentJobsAction: AppClientAction<IEmptyRequest,IJobSummaryModel[]>;
	readonly JobDetail: AppClientView<IGetJobDetailRequest>;
	readonly GetJobDetailAction: AppClientAction<IGetJobDetailRequest,ITriggeredJobDetailModel>;
	
	GetFailedJobs(errorOptions?: IActionErrorOptions) {
		return this.GetFailedJobsAction.execute({}, errorOptions || {});
	}
	GetRecentJobs(errorOptions?: IActionErrorOptions) {
		return this.GetRecentJobsAction.execute({}, errorOptions || {});
	}
	GetJobDetail(model: IGetJobDetailRequest, errorOptions?: IActionErrorOptions) {
		return this.GetJobDetailAction.execute(model, errorOptions || {});
	}
}