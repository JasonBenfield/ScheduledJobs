import { HubAppApi } from "@jasonbenfield/hubwebapp/Api/HubAppApi";
import { BasicComponent } from "@jasonbenfield/sharedwebapp/Components/BasicComponent";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { TextLinkComponent } from "@jasonbenfield/sharedwebapp/Components/TextLinkComponent";
import { ContextualClass } from "@jasonbenfield/sharedwebapp/ContextualClass";
import { AppEventSeverity } from '../../../Lib/Api/AppEventSeverity';
import { LogEntryItemView } from "./LogEntryItemView";

export class LogEntryItem extends BasicComponent {
    constructor(hubApi: HubAppApi, logEntry: IJobLogEntryModel, sourceLogEntry: ISourceLogEntryModel, view: LogEntryItemView) {
        super(view);
        const categoryComponent = new TextComponent(view.category);
        categoryComponent.setText(logEntry.Category);
        categoryComponent.syncTitleWithText();
        if (sourceLogEntry) {
            const sourceMessageComponent = new TextComponent(view.sourceMessage);
            sourceMessageComponent.setText(`[Source] ${sourceLogEntry.SourceLogEntry.Message}`);
            sourceMessageComponent.syncTitleWithText();
        }
        else {
            view.sourceMessage.hide();
        }
        const messageComponent = new TextComponent(view.message);
        messageComponent.setText(logEntry.Message);
        messageComponent.syncTitleWithText();
        const detailsComponent = new TextComponent(view.details);
        detailsComponent.setText(logEntry.Details);
        detailsComponent.syncTitleWithText();
        if (sourceLogEntry && sourceLogEntry.SourceLogEntry.RequestID) {
            const sourceLogEntryLink = new TextLinkComponent(view.sourceLogEntryLink);
            sourceLogEntryLink.setHref(
                hubApi.Logs.LogEntries.getUrl({
                    RequestID: sourceLogEntry.SourceLogEntry.RequestID,
                    InstallationID: null
                })
            );
        }
        else {
            view.sourceLogEntryLink.hide();
        }
        if (logEntry.Severity.Value > AppEventSeverity.values.Information.Value) {
            view.setContext(ContextualClass.danger);
        }

    }
}