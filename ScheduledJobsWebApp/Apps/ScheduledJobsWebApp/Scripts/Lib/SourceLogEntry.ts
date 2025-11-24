import { AppLogEntry } from "@jasonbenfield/hubwebapp/AppLogEntry";

export class SourceLogEntry {
    readonly logEntryID: number;
    readonly sourceLogEntry: AppLogEntry;

    constructor(source?: ISourceLogEntryModel) {
        this.logEntryID = source ? source.LogEntryID : 0;
        this.sourceLogEntry = new AppLogEntry(source && source.SourceLogEntry);
    }
}