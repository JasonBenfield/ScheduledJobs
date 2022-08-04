// Generated code
import { NumericValue } from '@jasonbenfield/sharedwebapp/NumericValue';
import { NumericValues } from '@jasonbenfield/sharedwebapp/NumericValues';

export class AppEventSeveritys extends NumericValues<AppEventSeverity> {
	constructor(
		public readonly NotSet: AppEventSeverity,
		public readonly CriticalError: AppEventSeverity,
		public readonly AccessDenied: AppEventSeverity,
		public readonly AppError: AppEventSeverity,
		public readonly ValidationFailed: AppEventSeverity,
		public readonly Information: AppEventSeverity
	) {
		super([NotSet,CriticalError,AccessDenied,AppError,ValidationFailed,Information]);
	}
}

export class AppEventSeverity extends NumericValue implements IAppEventSeverity {
	public static readonly values = new AppEventSeveritys(
		new AppEventSeverity(0, 'Not Set'),
		new AppEventSeverity(100, 'Critical Error'),
		new AppEventSeverity(80, 'Access Denied'),
		new AppEventSeverity(70, 'App Error'),
		new AppEventSeverity(60, 'Validation Failed'),
		new AppEventSeverity(50, 'Information')
	);
	
	private constructor(Value: number, DisplayText: string) {
		super(Value, DisplayText);
	}
}