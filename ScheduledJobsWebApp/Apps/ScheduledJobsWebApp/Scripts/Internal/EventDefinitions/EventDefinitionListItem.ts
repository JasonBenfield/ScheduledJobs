import { TextBlock } from "@jasonbenfield/sharedwebapp/Html/TextBlock";
import { EventDefinitionListItemView } from "./EventDefinitionListItemView";

export class EventDefinitionListItem {
    constructor(evtDef: IEventDefinitionModel, view: EventDefinitionListItemView) {
        new TextBlock(evtDef.EventKey.DisplayText, view.displayText);
    }
}