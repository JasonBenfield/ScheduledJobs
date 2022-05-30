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
		new DuplicateHandling(10, 'KeepOldest'),
		new DuplicateHandling(20, 'KeepNewest'),
		new DuplicateHandling(30, 'KeepAll')
	);
	
	private constructor(Value: number, DisplayText: string) {
		super(Value, DisplayText);
	}
}