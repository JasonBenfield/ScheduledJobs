import { ColumnCss } from "@jasonbenfield/sharedwebapp/ColumnCss";
import { ButtonCommandItem } from "@jasonbenfield/sharedwebapp/Command/ButtonCommandItem";
import { ContextualClass } from "@jasonbenfield/sharedwebapp/ContextualClass";
import { Row } from "@jasonbenfield/sharedwebapp/Grid/Row";
import { Block } from "@jasonbenfield/sharedwebapp/Html/Block";
import { FlexColumn } from "@jasonbenfield/sharedwebapp/Html/FlexColumn";
import { FlexColumnFill } from "@jasonbenfield/sharedwebapp/Html/FlexColumnFill";
import { TextBlockView } from "@jasonbenfield/sharedwebapp/Html/TextBlockView";
import { TextHeading3View } from "@jasonbenfield/sharedwebapp/Html/TextHeading3View";
import { TextSpanView } from "@jasonbenfield/sharedwebapp/Html/TextSpanView";
import { ListGroupView } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroupView";
import { MarginCss } from "@jasonbenfield/sharedwebapp/MarginCss";
import { MessageAlertView } from "@jasonbenfield/sharedwebapp/MessageAlertView";
import { TextCss } from "@jasonbenfield/sharedwebapp/TextCss";
import { ScheduledJobsTheme } from "../../ScheduledJobsTheme";
import { LogEntryItemView } from "./LogEntryItemView";

export class TaskDetailPanelView extends Block {
    constructor() {
        super();
        this.height100();
        let flexColumn = this.addContent(new FlexColumn());
        let navBar = flexColumn.addContent(new Block());
        let row = navBar.addContent(new Row());
        let col1 = row.addColumn();
        col1.setColumnCss(ColumnCss.xs(1));
        this.previousTaskButton = col1.addContent(
            new ButtonCommandItem()
                .configure(b => {
                    b.icon.setName('caret-left');
                    b.icon.makeFixedWidth();
                    b.setText(' ');
                    b.setTitle('Previous Task');
                    b.setContext(ContextualClass.secondary);
                    b.useOutlineStyle();
                })
        );
        let col2 = row.addColumn();
        col2.setTextCss(new TextCss().center());
        this.displayText = col2.addContent(new TextHeading3View());
        let col3 = row.addColumn();
        col3.setColumnCss(ColumnCss.xs(1));
        col3.setTextCss(new TextCss().end());
        this.nextTaskButton = col3.addContent(
            new ButtonCommandItem()
                .configure(b => {
                    b.icon.setName('caret-right');
                    b.icon.makeFixedWidth();
                    b.setText(' ');
                    b.setTitle('Next Task');
                    b.setContext(ContextualClass.secondary);
                    b.useOutlineStyle();
                })
        );
        let flexFill = flexColumn.addContent(new FlexColumnFill());
        let topBlock = flexFill.addContent(new Block());
        topBlock.setMargin(MarginCss.bottom(3));
        this.status = topBlock.addContent(new TextSpanView())
            .configure(b => b.setMargin(MarginCss.end(3)));
        this.timeStarted = topBlock.addContent(new TextSpanView())
            .configure(b => b.setMargin(MarginCss.end(3)));
        this.timeElapsed = topBlock.addContent(new TextSpanView());
        this.taskData = flexFill.addContent(new TextBlockView())
            .configure(b => b.setMargin(MarginCss.bottom(3)));
        this.logEntries = flexFill.addContent(new ListGroupView(() => new LogEntryItemView()));
        this.logEntries.setMargin(MarginCss.bottom(3));
        let buttonContainer = flexFill.addContent(new Block());
        buttonContainer.addCssName('d-grid');
        buttonContainer.addCssName('gap-2');
        buttonContainer.addCssName('mx-auto');
        buttonContainer.addCssFrom(ColumnCss.xs(6));
        this.retryTaskButton = buttonContainer.addContent(new ButtonCommandItem());
        this.retryTaskButton.icon.setName('repeat');
        this.retryTaskButton.setText('Retry Task');
        this.retryTaskButton.useOutlineStyle();
        this.retryTaskButton.setContext(ContextualClass.primary);

        this.cancelTaskButton = buttonContainer.addContent(new ButtonCommandItem());
        this.cancelTaskButton.icon.setName('times');
        this.cancelTaskButton.setText('Cancel Task');
        this.cancelTaskButton.useOutlineStyle();
        this.cancelTaskButton.setContext(ContextualClass.primary);

        this.alert = flexFill.addContent(new MessageAlertView());

        let toolbar = flexColumn.addContent(ScheduledJobsTheme.instance.commandToolbar.toolbar());
        this.backButton = toolbar.columnStart.addContent(ScheduledJobsTheme.instance.commandToolbar.backButton());
        this.backButton.setText('Back');
    }

    readonly previousTaskButton: ButtonCommandItem;
    readonly nextTaskButton: ButtonCommandItem;
    readonly displayText: ITextComponentView;
    readonly alert: MessageAlertView;
    readonly status: ITextComponentView;
    readonly timeStarted: ITextComponentView;
    readonly timeElapsed: ITextComponentView;
    readonly taskData: TextBlockView;
    readonly logEntries: ListGroupView;
    readonly retryTaskButton: ButtonCommandItem;
    readonly cancelTaskButton: ButtonCommandItem;
    readonly backButton: ButtonCommandItem;

}