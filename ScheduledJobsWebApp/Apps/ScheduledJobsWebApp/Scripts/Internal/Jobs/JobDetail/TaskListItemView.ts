import { ColumnCss } from "@jasonbenfield/sharedwebapp/ColumnCss";
import { ContextualClass } from "@jasonbenfield/sharedwebapp/ContextualClass";
import { MarginCss } from "@jasonbenfield/sharedwebapp/MarginCss";
import { TextCss } from "@jasonbenfield/sharedwebapp/TextCss";
import { BasicComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicComponentView";
import { BasicTextComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicTextComponentView";
import { ButtonListGroupItemView } from "@jasonbenfield/sharedwebapp/Views/ListGroup";
import { RowView } from "@jasonbenfield/sharedwebapp/Views/RowView";
import { TextSpanView } from "@jasonbenfield/sharedwebapp/Views/TextSpanView";

export class TaskListItemView extends ButtonListGroupItemView {
    constructor(container: BasicComponentView) {
        super(container);
        const row = this.addView(RowView);
        const col1 = row.addColumn();
        this.generation = col1.addView(TextSpanView);
        this.generation.setMargin(MarginCss.end(1));
        this.displayText = col1.addView(TextSpanView);
        const col2 = row.addColumn();
        this.timeStarted = col2.addView(TextSpanView);
        this.timeStarted.setMargin(MarginCss.end(3));
        this.timeElapsed = col2.addView(TextSpanView);
        let col3 = row.addColumn();
        col3.setColumnCss(ColumnCss.xs(2));
        this.status = col3.addView(TextSpanView);
        let col4 = row.addColumn();
        col4.setColumnCss(ColumnCss.xs(2));
        col4.setTextCss(new TextCss().end());
        this.logEntryCount = col4.addView(TextSpanView);
        this.logEntryCount.addCssName('badge');
        this.logEntryCount.setBackgroundContext(ContextualClass.secondary);
    }

    readonly generation: TextSpanView;
    readonly displayText: BasicTextComponentView;
    readonly status: BasicTextComponentView;
    readonly timeStarted: TextSpanView;
    readonly timeElapsed: TextSpanView;
    readonly logEntryCount: TextSpanView;

    failed() {
        this.setContext(ContextualClass.danger);
        this.logEntryCount.setBackgroundContext(ContextualClass.danger);
    }
}