import { BasicComponent } from "@jasonbenfield/sharedwebapp/Components/BasicComponent";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { JobDefinition } from "../../Lib/JobDefinition";
import { JobDefinitionListItemView } from "./JobDefinitionListItemView";

export class JobDefinitionListItem extends BasicComponent {
    constructor(readonly jobDefinition: JobDefinition, view: JobDefinitionListItemView) {
        super(view);
        const displayText = this.addComponent(new TextComponent(view.displayText));
        displayText.setText(jobDefinition.jobKey.displayText);
        displayText.syncTitleWithText();
    }
}