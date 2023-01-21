// Generated code
import { NumericValue } from '@jasonbenfield/sharedwebapp/NumericValue';
import { NumericValues } from '@jasonbenfield/sharedwebapp/NumericValues';

export class JobTaskStatuss extends NumericValues<JobTaskStatus> {
	constructor(
		public readonly NotSet: JobTaskStatus,
		public readonly Failed: JobTaskStatus,
		public readonly Retry: JobTaskStatus,
		public readonly Skip: JobTaskStatus,
		public readonly Running: JobTaskStatus,
		public readonly Pending: JobTaskStatus,
		public readonly Canceled: JobTaskStatus,
		public readonly Completed: JobTaskStatus
	) {
		super([NotSet,Failed,Retry,Skip,Running,Pending,Canceled,Completed]);
	}
}

export class JobTaskStatus extends NumericValue implements IJobTaskStatus {
	public static readonly values = new JobTaskStatuss(
		new JobTaskStatus(0, 'Not Set'),
		new JobTaskStatus(10, 'Failed'),
		new JobTaskStatus(20, 'Retry'),
		new JobTaskStatus(25, 'Skip'),
		new JobTaskStatus(30, 'Running'),
		new JobTaskStatus(40, 'Pending'),
		new JobTaskStatus(50, 'Canceled'),
		new JobTaskStatus(60, 'Completed')
	);
	
	private constructor(Value: number, DisplayText: string) {
		super(Value, DisplayText);
	}
}