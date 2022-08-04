import { ColumnCss } from "@jasonbenfield/sharedwebapp/ColumnCss";
import { ContextualClass } from "@jasonbenfield/sharedwebapp/ContextualClass";
import { CssLengthUnit } from "@jasonbenfield/sharedwebapp/CssLengthUnit";
import { MarginCss } from "@jasonbenfield/sharedwebapp/MarginCss";
import { TextCss } from "@jasonbenfield/sharedwebapp/TextCss";
import { BasicComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicComponentView";
import { BasicTextComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicTextComponentView";
import { BlockView } from "@jasonbenfield/sharedwebapp/Views/BlockView";
import { ButtonCommandView } from "@jasonbenfield/sharedwebapp/Views/Command";
import { GridView } from "@jasonbenfield/sharedwebapp/Views/Grid";
import { ListGroupView } from "@jasonbenfield/sharedwebapp/Views/ListGroup";
import { MessageAlertView } from "@jasonbenfield/sharedwebapp/Views/MessageAlertView";
import { ModalConfirmView } from "@jasonbenfield/sharedwebapp/Views/Modal";
import { RowView } from "@jasonbenfield/sharedwebapp/Views/RowView";
import { TextBlockView } from "@jasonbenfield/sharedwebapp/Views/TextBlockView";
import { TextHeading3View } from "@jasonbenfield/sharedwebapp/Views/TextHeadings";
import { TextSpanView } from "@jasonbenfield/sharedwebapp/Views/TextSpanView";
import { ToolbarView } from "@jasonbenfield/sharedwebapp/Views/ToolbarView";
import { ScheduledJobsTheme } from "../../ScheduledJobsTheme";
import { LogEntryItemView } from "./LogEntryItemView";

export class TaskDetailPanelView extends GridView {
    constructor(container: BasicComponentView) {
        super(container);
        this.height100();
        this.layout();
        this.setTemplateRows(CssLengthUnit.flex(1), CssLengthUnit.auto());
        const mainContent = ScheduledJobsTheme.instance.mainContent(this.addCell());
        const navBar = mainContent.addView(BlockView);
        navBar.setMargin(MarginCss.bottom(3));
        const row = navBar.addView(RowView);
        const col1 = row.addColumn();
        col1.setColumnCss(ColumnCss.xs(1));
        this.previousTaskButton = col1.addView(ButtonCommandView)
            .configure(b => {
                b.icon.solidStyle('caret-left');
                b.icon.makeFixedWidth();
                b.setText(' ');
                b.setTitle('Previous Task');
                b.useOutlineStyle(ContextualClass.secondary);
            });
        const col2 = row.addColumn();
        col2.setTextCss(new TextCss().center());
        this.displayText = col2.addView(TextHeading3View);
        const col3 = row.addColumn();
        col3.setColumnCss(ColumnCss.xs(1));
        col3.setTextCss(new TextCss().end());
        this.nextTaskButton = col3.addView(ButtonCommandView)
            .configure(b => {
                b.icon.solidStyle('caret-right');
                b.icon.makeFixedWidth();
                b.setText(' ');
                b.setTitle('Next Task');
                b.useOutlineStyle(ContextualClass.secondary);
            });
        const topBlock = mainContent.addView(BlockView);
        topBlock.setMargin(MarginCss.bottom(3));
        this.status = topBlock.addView(TextSpanView)
            .configure(b => b.setMargin(MarginCss.end(3)));
        this.timeStarted = topBlock.addView(TextSpanView)
            .configure(b => b.setMargin(MarginCss.end(3)));
        this.timeElapsed = topBlock.addView(TextSpanView);
        this.taskData = mainContent.addView(TextBlockView)
            .configure(b => b.setMargin(MarginCss.bottom(3)));
        this.logEntries = mainContent.addView(ListGroupView);
        this.logEntries.setItemViewType(LogEntryItemView);
        this.logEntries.setMargin(MarginCss.bottom(3));
        const buttonContainer = mainContent.addView(BlockView);
        buttonContainer.addCssName('d-grid');
        buttonContainer.addCssName('gap-2');
        buttonContainer.addCssName('mx-auto');
        buttonContainer.addCssFrom(ColumnCss.xs(6));
        this.retryTaskButton = buttonContainer.addView(ButtonCommandView);
        this.retryTaskButton.icon.solidStyle('repeat');
        this.retryTaskButton.setText('Retry Task');
        this.retryTaskButton.useOutlineStyle(ContextualClass.primary);

        this.cancelTaskButton = buttonContainer.addView(ButtonCommandView);
        this.cancelTaskButton.icon.solidStyle('times');
        this.cancelTaskButton.setText('Cancel Task');
        this.cancelTaskButton.useOutlineStyle(ContextualClass.primary);

        this.alert = mainContent.addView(MessageAlertView);

        const toolbar = ScheduledJobsTheme.instance.commandToolbar.toolbar(this.addCell().addView(ToolbarView));
        this.backButton = ScheduledJobsTheme.instance.commandToolbar.backButton(
            toolbar.columnStart.addView(ButtonCommandView)
        );
        this.backButton.setText('Back');
        this.modalConfirm = this.addView(ModalConfirmView);
    }

    readonly previousTaskButton: ButtonCommandView;
    readonly nextTaskButton: ButtonCommandView;
    readonly displayText: BasicTextComponentView;
    readonly alert: MessageAlertView;
    readonly status: BasicTextComponentView;
    readonly timeStarted: BasicTextComponentView;
    readonly timeElapsed: BasicTextComponentView;
    readonly taskData: TextBlockView;
    readonly logEntries: ListGroupView;
    readonly retryTaskButton: ButtonCommandView;
    readonly cancelTaskButton: ButtonCommandView;
    readonly backButton: ButtonCommandView;
    readonly modalConfirm: ModalConfirmView;

}