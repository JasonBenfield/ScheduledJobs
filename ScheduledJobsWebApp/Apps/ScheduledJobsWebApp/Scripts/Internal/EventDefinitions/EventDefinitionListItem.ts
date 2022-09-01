import { BasicComponent } from "@jasonbenfield/sharedwebapp/Components/BasicComponent";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { EventDefinitionListItemView } from "./EventDefinitionListItemView";

export class EventDefinitionListItem extends BasicComponent {
    constructor(readonly evtDef: IEventDefinitionModel, view: EventDefinitionListItemView) {
        super(view);
        const displayText = new TextComponent(view.displayText);
        displayText.setText(evtDef.EventKey.DisplayText);
        displayText.syncTitleWithText();
    }
}