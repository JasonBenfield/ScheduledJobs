// Generated code
import { NumericValue } from '@jasonbenfield/sharedwebapp/NumericValue';
import { NumericValues } from '@jasonbenfield/sharedwebapp/NumericValues';

export class DuplicateHandlings extends NumericValues<DuplicateHandling> {
	constructor(
		public readonly Ignore: DuplicateHandling,
		public readonly KeepOldest: DuplicateHandling,
		public readonly KeepNewest: DuplicateHandling,
		public readonly KeepAll: DuplicateHandling
	) {
		super([Ignore,KeepOldest,KeepNewest,KeepAll]);
	}
}

export class DuplicateHandling extends NumericValue implements IDuplicateHandling {
	public static readonly values = new DuplicateHandlings(
		new DuplicateHandling(0, 'Ignore'),
		new DuplicateHandling(10, 'Keep Oldest'),
		new DuplicateHandling(20, 'Keep Newest'),
		new DuplicateHandling(30, 'Keep All')
	);
	
	private constructor(Value: number, DisplayText: string) {
		super(Value, DisplayText);
	}
	
	equalsAny: (...other: this[] | IDuplicateHandling[] | number[] | string[]) => boolean;
	
	equals: (other: this | IDuplicateHandling | number | string) => boolean;
}