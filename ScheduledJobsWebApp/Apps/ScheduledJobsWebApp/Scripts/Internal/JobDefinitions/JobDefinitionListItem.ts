import { TextBlock } from "@jasonbenfield/sharedwebapp/Html/TextBlock";
import { JobDefinitionListItemView } from "./JobDefinitionListItemView";

export class JobDefinitionListItem {
    constructor(readonly jobDefinition: IJobDefinitionModel, view: JobDefinitionListItemView) {
        new TextBlock(jobDefinition.JobKey.DisplayText, view.displayText);
    }
}