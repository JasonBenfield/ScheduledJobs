import { BasicComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicComponentView";
import { BasicTextComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicTextComponentView";
import { ButtonListGroupItemView } from "@jasonbenfield/sharedwebapp/Views/ListGroup";
import { TextBlockView } from "@jasonbenfield/sharedwebapp/Views/TextBlockView";

export class EventDefinitionListItemView extends ButtonListGroupItemView {
    readonly displayText: BasicTextComponentView;

    constructor(container: BasicComponentView) {
        super(container);
        this.displayText = this.addView(TextBlockView);
    }
}