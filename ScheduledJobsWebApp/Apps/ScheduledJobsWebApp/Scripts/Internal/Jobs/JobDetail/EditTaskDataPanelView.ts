import { CssLengthUnit } from "@jasonbenfield/sharedwebapp/CssLengthUnit";
import { BasicComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicComponentView";
import { ButtonCommandView } from "@jasonbenfield/sharedwebapp/Views/Command";
import { FormGroupTextAreaView } from "@jasonbenfield/sharedwebapp/Views/FormGroup";
import { GridView } from "@jasonbenfield/sharedwebapp/Views/Grid";
import { MessageAlertView } from "@jasonbenfield/sharedwebapp/Views/MessageAlertView";
import { ToolbarView } from "@jasonbenfield/sharedwebapp/Views/ToolbarView";
import { ScheduledJobsTheme } from "../../ScheduledJobsTheme";
import { FormGroupContainerView } from "@jasonbenfield/sharedwebapp/Views/FormGroupContainerView";

export class EditTaskDataPanelView extends GridView {
    readonly taskDataFormGroup: FormGroupTextAreaView;
    readonly alert: MessageAlertView;
    readonly cancelButton: ButtonCommandView;
    readonly saveButton: ButtonCommandView;

    constructor(container: BasicComponentView) {
        super(container);
        this.height100();
        this.styleAsLayout();
        this.setTemplateRows(CssLengthUnit.flex(1), CssLengthUnit.auto());
        const mainContent = ScheduledJobsTheme.instance.mainContent(this.addCell());
        const formGroupContainer = mainContent.addView(FormGroupContainerView);
        this.taskDataFormGroup = formGroupContainer.addFormGroupTextAreaView();
        this.taskDataFormGroup.textArea.setRows(10);
        this.alert = mainContent.addView(MessageAlertView);
        const toolbar = ScheduledJobsTheme.instance.commandToolbar.toolbar(this.addCell().addView(ToolbarView));
        this.cancelButton = ScheduledJobsTheme.instance.commandToolbar.cancelButton(
            toolbar.columnEnd.addView(ButtonCommandView)
        );
        this.saveButton = ScheduledJobsTheme.instance.commandToolbar.saveButton(
            toolbar.columnEnd.addView(ButtonCommandView)
        );
    }
}