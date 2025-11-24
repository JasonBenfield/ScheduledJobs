import { BasicComponent } from "@jasonbenfield/sharedwebapp/Components/BasicComponent";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { EventDefinitionListItemView } from "./EventDefinitionListItemView";
import { EventDefinition } from "../../Lib/EventDefinition";

export class EventDefinitionListItem extends BasicComponent {
    constructor(readonly evtDef: EventDefinition, view: EventDefinitionListItemView) {
        super(view);
        const displayText = new TextComponent(view.displayText);
        displayText.setText(evtDef.eventKey.displayText);
        displayText.syncTitleWithText();
    }
}