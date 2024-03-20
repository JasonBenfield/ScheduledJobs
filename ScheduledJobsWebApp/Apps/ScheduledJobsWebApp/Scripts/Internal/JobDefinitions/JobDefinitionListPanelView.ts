import { CssLengthUnit } from "@jasonbenfield/sharedwebapp/CssLengthUnit";
import { BasicComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicComponentView";
import { ButtonCommandView } from "@jasonbenfield/sharedwebapp/Views/Command";
import { GridView } from "@jasonbenfield/sharedwebapp/Views/Grid";
import { ButtonListGroupView } from "@jasonbenfield/sharedwebapp/Views/ListGroup";
import { MessageAlertView } from "@jasonbenfield/sharedwebapp/Views/MessageAlertView";
import { ToolbarView } from "@jasonbenfield/sharedwebapp/Views/ToolbarView";
import { ScheduledJobsTheme } from "../ScheduledJobsTheme";
import { JobDefinitionListItemView } from "./JobDefinitionListItemView";

export class JobDefinitionListPanelView extends GridView {
    readonly alert: MessageAlertView;
    readonly jobDefinitionListView: ButtonListGroupView<JobDefinitionListItemView>;
    readonly menuButton: ButtonCommandView;
    readonly refreshButton: ButtonCommandView;

    constructor(container: BasicComponentView) {
        super(container);
        this.height100();
        this.styleAsLayout();
        this.setTemplateRows(CssLengthUnit.flex(1), CssLengthUnit.auto());
        const mainContent = ScheduledJobsTheme.instance.mainContent(this.addCell());
        this.alert = mainContent.addView(MessageAlertView);
        this.jobDefinitionListView = mainContent.addButtonListGroup(JobDefinitionListItemView);
        const toolbar = ScheduledJobsTheme.instance.commandToolbar.toolbar(
            this.addCell().addView(ToolbarView)
        );
        this.menuButton = ScheduledJobsTheme.instance.commandToolbar.menuButton(
            toolbar.columnStart.addView(ButtonCommandView)
        );
        this.refreshButton = ScheduledJobsTheme.instance.commandToolbar.refreshButton(
            toolbar.columnStart.addView(ButtonCommandView)
        );
    }
}