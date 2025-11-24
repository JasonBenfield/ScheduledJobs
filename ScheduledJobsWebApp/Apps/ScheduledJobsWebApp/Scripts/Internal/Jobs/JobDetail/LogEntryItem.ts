import { HubAppClient } from "@jasonbenfield/hubwebapp/Http/HubAppClient";
import { BasicComponent } from "@jasonbenfield/sharedwebapp/Components/BasicComponent";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { TextLinkComponent } from "@jasonbenfield/sharedwebapp/Components/TextLinkComponent";
import { ContextualClass } from "@jasonbenfield/sharedwebapp/ContextualClass";
import { AppEventSeverity } from '../../../Lib/Http/AppEventSeverity';
import { JobLogEntry } from "../../../Lib/JobLogEntry";
import { SourceLogEntry } from "../../../Lib/SourceLogEntry";
import { LogEntryItemView } from "./LogEntryItemView";

export class LogEntryItem extends BasicComponent {
    constructor(hubClient: HubAppClient, logEntry: JobLogEntry, sourceLogEntry: SourceLogEntry, view: LogEntryItemView) {
        super(view);
        const categoryComponent = this.addComponent(new TextComponent(view.category));
        categoryComponent.setText(logEntry.category);
        categoryComponent.syncTitleWithText();
        if (sourceLogEntry) {
            const sourceMessageComponent = this.addComponent(new TextComponent(view.sourceMessage));
            sourceMessageComponent.setText(`[Source] ${sourceLogEntry.sourceLogEntry.message}`);
            sourceMessageComponent.syncTitleWithText();
        }
        else {
            view.sourceMessage.hide();
        }
        const messageComponent = this.addComponent(new TextComponent(view.message));
        messageComponent.setText(logEntry.message);
        messageComponent.syncTitleWithText();
        const detailsComponent = this.addComponent(new TextComponent(view.details));
        detailsComponent.setText(logEntry.details);
        detailsComponent.syncTitleWithText();
        if (sourceLogEntry && sourceLogEntry.sourceLogEntry.requestID) {
            const sourceLogEntryLink = this.addComponent(new TextLinkComponent(view.sourceLogEntryLink));
            sourceLogEntryLink.setHref(
                hubClient.Logs.LogEntries.getUrl({
                    RequestID: sourceLogEntry.sourceLogEntry.requestID,
                    InstallationID: null
                })
            );
        }
        else {
            view.sourceLogEntryLink.hide();
        }
        if (logEntry.severity.Value > AppEventSeverity.values.Information.Value) {
            view.setContext(ContextualClass.danger);
        }

    }
}