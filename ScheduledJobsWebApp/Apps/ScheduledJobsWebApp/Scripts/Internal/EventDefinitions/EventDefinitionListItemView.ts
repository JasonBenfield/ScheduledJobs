import { TextBlockView } from "@jasonbenfield/sharedwebapp/Html/TextBlockView";
import { ButtonListGroupItemView } from "@jasonbenfield/sharedwebapp/ListGroup/ButtonListGroupItemView";

export class EventDefinitionListItemView extends ButtonListGroupItemView {
    readonly displayText: ITextComponentView;

    constructor() {
        super();
        this.displayText = this.addContent(new TextBlockView());
    }
}