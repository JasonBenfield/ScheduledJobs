﻿import { CssLengthUnit } from "@jasonbenfield/sharedwebapp/CssLengthUnit";
import { PaddingCss } from "@jasonbenfield/sharedwebapp/PaddingCss";
import { BasicComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicComponentView";
import { BasicTextComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicTextComponentView";
import { BlockView } from "@jasonbenfield/sharedwebapp/Views/BlockView";
import { ButtonCommandView } from "@jasonbenfield/sharedwebapp/Views/Command";
import { FormGroupLinkView } from "@jasonbenfield/sharedwebapp/Views/FormGroup";
import { FormGroupContainerView } from "@jasonbenfield/sharedwebapp/Views/FormGroupContainerView";
import { GridView } from "@jasonbenfield/sharedwebapp/Views/Grid";
import { ButtonListGroupView } from "@jasonbenfield/sharedwebapp/Views/ListGroup";
import { MessageAlertView } from "@jasonbenfield/sharedwebapp/Views/MessageAlertView";
import { TextHeading1View } from "@jasonbenfield/sharedwebapp/Views/TextHeadings";
import { ToolbarView } from "@jasonbenfield/sharedwebapp/Views/ToolbarView";
import { ScheduledJobsTheme } from "../../ScheduledJobsTheme";
import { TaskListItemView } from "./TaskListItemView";
import { LinkWithTextView } from "@jasonbenfield/sharedwebapp/Views/LinkWithTextView";

export class JobDetailPanelView extends GridView {
    readonly alert: MessageAlertView;
    private readonly jobBlock: BlockView;
    readonly jobDisplayText: BasicTextComponentView;
    private readonly triggeredByFormGroup: FormGroupLinkView;
    readonly triggeredByLink: LinkWithTextView;
    readonly tasks: ButtonListGroupView<TaskListItemView>;
    readonly menuButton: ButtonCommandView;
    readonly refreshButton: ButtonCommandView;

    constructor(container: BasicComponentView) {
        super(container);
        this.height100();
        this.styleAsLayout();
        this.setTemplateRows(CssLengthUnit.flex(1), CssLengthUnit.auto());
        const mainContent = ScheduledJobsTheme.instance.mainContent(this.addCell());
        this.alert = mainContent.addView(MessageAlertView);
        this.jobBlock = mainContent.addView(BlockView);
        this.jobBlock.addCssName('container');
        this.jobDisplayText = this.jobBlock.addView(TextHeading1View);
        const formGroupContainer = this.jobBlock.addView(FormGroupContainerView);
        this.triggeredByFormGroup = formGroupContainer.addFormGroupLinkView();
        this.triggeredByFormGroup.caption.setText('Triggered By');
        this.triggeredByLink = this.triggeredByFormGroup.linkView;
        this.jobBlock.setPadding(PaddingCss.xs({ top: 3, bottom: 3 }));
        this.tasks = mainContent.addButtonListGroup(TaskListItemView);
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

    showJob() {
        this.jobBlock.show();
        this.tasks.show();
    }

    hideJob() {
        this.jobBlock.hide();
        this.tasks.hide();
    }
}