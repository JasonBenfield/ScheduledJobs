import { TextBlock } from "@jasonbenfield/sharedwebapp/Html/TextBlock";
import { LogEntryItemView } from "./LogEntryItemView";
import { AppEventSeverity } from '../../../ScheduledJobs/Api/AppEventSeverity';
import { ContextualClass } from "@jasonbenfield/sharedwebapp/ContextualClass";

export class LogEntryItem {
    constructor(logEntry: ILogEntryModel, view: LogEntryItemView) {
        new TextBlock(logEntry.Category, view.category).syncTitleWithText();
        new TextBlock(logEntry.Message, view.message).syncTitleWithText();
        new TextBlock(logEntry.Details, view.details).syncTitleWithText();
        if (logEntry.Severity.Value > AppEventSeverity.values.Information.Value) {
            view.setContext(ContextualClass.danger);
        }
    }
}