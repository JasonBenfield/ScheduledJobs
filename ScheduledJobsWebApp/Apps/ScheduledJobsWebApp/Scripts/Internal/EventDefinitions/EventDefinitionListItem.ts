import { BasicComponent } from "@jasonbenfield/sharedwebapp/Components/BasicComponent";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { EventDefinitionListItemView } from "./EventDefinitionListItemView";

export class EventDefinitionListItem extends BasicComponent {
    constructor(readonly evtDef: IEventDefinitionModel, view: EventDefinitionListItemView) {
        super(view);
        new TextComponent(view.displayText).setText(evtDef.EventKey.DisplayText);
    }
}