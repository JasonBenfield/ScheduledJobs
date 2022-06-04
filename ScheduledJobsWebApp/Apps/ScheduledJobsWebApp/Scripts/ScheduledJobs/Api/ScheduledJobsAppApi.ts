// Generated code

import { AppApi } from "@jasonbenfield/sharedwebapp/Api/AppApi";
import { AppApiEvents } from "@jasonbenfield/sharedwebapp/Api/AppApiEvents";
import { UserGroup } from "./UserGroup";
import { UserCacheGroup } from "./UserCacheGroup";
import { HomeGroup } from "./HomeGroup";
import { RecurringGroup } from "./RecurringGroup";
import { EventsGroup } from "./EventsGroup";
import { JobInquiryGroup } from "./JobInquiryGroup";
import { JobsGroup } from "./JobsGroup";


export class ScheduledJobsAppApi extends AppApi {
	constructor(events: AppApiEvents) {
		super(events, 'ScheduledJobs');
		this.User = this.addGroup((evts, resourceUrl) => new UserGroup(evts, resourceUrl));
		this.UserCache = this.addGroup((evts, resourceUrl) => new UserCacheGroup(evts, resourceUrl));
		this.Home = this.addGroup((evts, resourceUrl) => new HomeGroup(evts, resourceUrl));
		this.Recurring = this.addGroup((evts, resourceUrl) => new RecurringGroup(evts, resourceUrl));
		this.Events = this.addGroup((evts, resourceUrl) => new EventsGroup(evts, resourceUrl));
		this.JobInquiry = this.addGroup((evts, resourceUrl) => new JobInquiryGroup(evts, resourceUrl));
		this.Jobs = this.addGroup((evts, resourceUrl) => new JobsGroup(evts, resourceUrl));
	}
	
	readonly User: UserGroup;
	readonly UserCache: UserCacheGroup;
	readonly Home: HomeGroup;
	readonly Recurring: RecurringGroup;
	readonly Events: EventsGroup;
	readonly JobInquiry: JobInquiryGroup;
	readonly Jobs: JobsGroup;
}