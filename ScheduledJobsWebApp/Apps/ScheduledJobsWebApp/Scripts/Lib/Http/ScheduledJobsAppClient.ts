// Generated code

import { AppClient } from "@jasonbenfield/sharedwebapp/Http/AppClient";
import { AppClientEvents } from "@jasonbenfield/sharedwebapp/Http/AppClientEvents";
import { AppClientQuery } from "@jasonbenfield/sharedwebapp/Http/AppClientQuery";
import { HomeGroup } from "./HomeGroup";
import { RecurringGroup } from "./RecurringGroup";
import { EventDefinitionsGroup } from "./EventDefinitionsGroup";
import { EventInquiryGroup } from "./EventInquiryGroup";
import { EventsGroup } from "./EventsGroup";
import { JobDefinitionsGroup } from "./JobDefinitionsGroup";
import { JobInquiryGroup } from "./JobInquiryGroup";
import { JobsGroup } from "./JobsGroup";
import { TasksGroup } from "./TasksGroup";


export class ScheduledJobsAppClient extends AppClient {
	constructor(events: AppClientEvents) {
		super(
			events, 
			'ScheduledJobs', 
			pageContext.EnvironmentName === 'Production' || pageContext.EnvironmentName === 'Staging' ? 'V24' : 'Current'
		);
		this.Home = this.addGroup((evts, resourceUrl) => new HomeGroup(evts, resourceUrl));
		this.Recurring = this.addGroup((evts, resourceUrl) => new RecurringGroup(evts, resourceUrl));
		this.EventDefinitions = this.addGroup((evts, resourceUrl) => new EventDefinitionsGroup(evts, resourceUrl));
		this.EventInquiry = this.addGroup((evts, resourceUrl) => new EventInquiryGroup(evts, resourceUrl));
		this.Events = this.addGroup((evts, resourceUrl) => new EventsGroup(evts, resourceUrl));
		this.JobDefinitions = this.addGroup((evts, resourceUrl) => new JobDefinitionsGroup(evts, resourceUrl));
		this.JobInquiry = this.addGroup((evts, resourceUrl) => new JobInquiryGroup(evts, resourceUrl));
		this.Jobs = this.addGroup((evts, resourceUrl) => new JobsGroup(evts, resourceUrl));
		this.Tasks = this.addGroup((evts, resourceUrl) => new TasksGroup(evts, resourceUrl));
	}
	
	readonly Home: HomeGroup;
	readonly Recurring: RecurringGroup;
	readonly EventDefinitions: EventDefinitionsGroup;
	readonly EventInquiry: EventInquiryGroup;
	readonly Events: EventsGroup;
	readonly JobDefinitions: JobDefinitionsGroup;
	readonly JobInquiry: JobInquiryGroup;
	readonly Jobs: JobsGroup;
	readonly Tasks: TasksGroup;
}