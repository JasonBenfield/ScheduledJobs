import { DateTimeOffset } from "@jasonbenfield/sharedwebapp/Common";
import { AppEventSeverity } from "./Http/AppEventSeverity";

export class JobLogEntry {
    readonly id: number;
    readonly severity: AppEventSeverity;
    readonly timeOccurred: DateTimeOffset;
    readonly category: string;
    readonly message: string;
    readonly details: string;
    readonly sourceEventKey: string;

    constructor(source?: IJobLogEntryModel) {
        this.id = source ? source.ID : 0;
        this.severity = source ? AppEventSeverity.values.value(source.Severity) : AppEventSeverity.values.NotSet;
        this.timeOccurred = source ? source.TimeOccurred : DateTimeOffset.max();
        this.category = source ? source.Category : "";
        this.message = source ? source.Message : "";
        this.details = source ? source.Details : "";
        this.sourceEventKey = source ? source.SourceEventKey : "";
    }
}