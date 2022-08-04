import { BasicComponent } from "@jasonbenfield/sharedwebapp/Components/BasicComponent";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { ContextualClass } from "@jasonbenfield/sharedwebapp/ContextualClass";
import { AppEventSeverity } from '../../../Lib/Api/AppEventSeverity';
import { LogEntryItemView } from "./LogEntryItemView";

export class LogEntryItem extends BasicComponent {
    constructor(logEntry: ILogEntryModel, view: LogEntryItemView) {
        super(view);
        const category = new TextComponent(view.category);
        category.setText(logEntry.Category);
        category.syncTitleWithText();
        const message = new TextComponent(view.message);
        message.setText(logEntry.Message);
        message.syncTitleWithText();
        const details = new TextComponent(view.details);
        details.setText(logEntry.Details);
        details.syncTitleWithText();
        if (logEntry.Severity.Value > AppEventSeverity.values.Information.Value) {
            view.setContext(ContextualClass.danger);
        }
    }
}