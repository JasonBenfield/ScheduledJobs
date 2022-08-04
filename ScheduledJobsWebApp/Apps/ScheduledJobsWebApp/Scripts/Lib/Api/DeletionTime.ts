// Generated code
import { NumericValue } from '@jasonbenfield/sharedwebapp/NumericValue';
import { NumericValues } from '@jasonbenfield/sharedwebapp/NumericValues';

export class DeletionTimes extends NumericValues<DeletionTime> {
	constructor(
		public readonly JobDefault: DeletionTime,
		public readonly Immediately: DeletionTime,
		public readonly NextDay: DeletionTime
	) {
		super([JobDefault,Immediately,NextDay]);
	}
}

export class DeletionTime extends NumericValue implements IDeletionTime {
	public static readonly values = new DeletionTimes(
		new DeletionTime(0, 'JobDefault'),
		new DeletionTime(10, 'Immediately'),
		new DeletionTime(20, 'NextDay')
	);
	
	private constructor(Value: number, DisplayText: string) {
		super(Value, DisplayText);
	}
}