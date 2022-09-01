import { BasicComponent } from "@jasonbenfield/sharedwebapp/Components/BasicComponent";
import { TextComponent } from "@jasonbenfield/sharedwebapp/Components/TextComponent";
import { JobDefinitionListItemView } from "./JobDefinitionListItemView";

export class JobDefinitionListItem extends BasicComponent {
    constructor(readonly jobDefinition: IJobDefinitionModel, view: JobDefinitionListItemView) {
        super(view);
        const displayText = new TextComponent(view.displayText);
        displayText.setText(jobDefinition.JobKey.DisplayText);
        displayText.syncTitleWithText();
    }
}