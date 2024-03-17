import { CssLengthUnit } from "@jasonbenfield/sharedwebapp/CssLengthUnit";
import { TextCss } from "@jasonbenfield/sharedwebapp/TextCss";
import { BasicComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicComponentView";
import { ButtonCommandView } from "@jasonbenfield/sharedwebapp/Views/Command";
import { SimpleFieldFormGroupInputView } from "@jasonbenfield/sharedwebapp/Views/FormGroup";
import { FormView } from "@jasonbenfield/sharedwebapp/Views/FormView";
import { GridView } from "@jasonbenfield/sharedwebapp/Views/Grid";
import { ToolbarView } from "@jasonbenfield/sharedwebapp/Views/ToolbarView";
import { ScheduledJobsTheme } from "../ScheduledJobsTheme";
import { FormGroupContainerView } from "@jasonbenfield/sharedwebapp/Views/FormGroupContainerView";

export class SelectSourceKeyPanelView extends GridView {
    private readonly form: FormView;
    readonly sourceKey: SimpleFieldFormGroupInputView;
    readonly backButton: ButtonCommandView;
    readonly nextButton: ButtonCommandView;

    constructor(container: BasicComponentView) {
        super(container);
        this.height100();
        this.styleAsLayout();
        this.setTemplateRows(CssLengthUnit.flex(1), CssLengthUnit.auto());
        const mainContent = ScheduledJobsTheme.instance.mainContent(this.addCell());
        this.form = mainContent.addView(FormView);
        this.form.addOffscreenSubmit();
        const formGroupContainer = this.form.addView(FormGroupContainerView);
        this.sourceKey = formGroupContainer.addFormGroup(SimpleFieldFormGroupInputView);
        this.sourceKey.captionCell.setTextCss(new TextCss().end());
        const toolbar = ScheduledJobsTheme.instance.commandToolbar.toolbar(
            this.addCell().addView(ToolbarView)
        );
        this.backButton = ScheduledJobsTheme.instance.commandToolbar.backButton(
            toolbar.columnStart.addView(ButtonCommandView)
        );
        this.nextButton = ScheduledJobsTheme.instance.commandToolbar.nextButton(
            toolbar.columnEnd.addView(ButtonCommandView)
        );
    }

    handleFormSubmitted(action: (el: HTMLElement, evt: JQuery.Event) => void) {
        this.form.onSubmit()
            .execute(action)
            .subscribe();
    }
}