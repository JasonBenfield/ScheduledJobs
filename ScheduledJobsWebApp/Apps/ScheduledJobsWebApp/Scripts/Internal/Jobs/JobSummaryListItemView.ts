import { ContextualClass } from "@jasonbenfield/sharedwebapp/ContextualClass";
import { RowView } from "@jasonbenfield/sharedwebapp/Views/RowView";
import { TextSpanView } from "@jasonbenfield/sharedwebapp/Views/TextSpanView";
import { LinkListGroupItemView } from "@jasonbenfield/sharedwebapp/Views/ListGroup";
import { TextCss } from "@jasonbenfield/sharedwebapp/TextCss";
import { BasicComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicComponentView";

export class JobSummaryListItemView extends LinkListGroupItemView {

    constructor(container: BasicComponentView) {
        super(container);
        const row = this.addView(RowView);
        const col1 = row.addColumn();
        this.displayText = col1.addView(TextSpanView);
        const col2 = row.addColumn();
        this.timeStarted = col2
            .addView(TextSpanView);
        this.timeElapsed = col2.addView(TextSpanView);
        const col3 = row.addColumn();
        this.status = col3.addView(TextSpanView);
        const col4 = row.addColumn();
        col4.setTextCss(new TextCss().end());
        this.taskCount = col4.addView(TextSpanView);
        this.taskCount.addCssName('badge');
        this.taskCount.setBackgroundContext(ContextualClass.secondary);
    }

    readonly displayText: TextSpanView;
    readonly status: TextSpanView;
    readonly timeStarted: TextSpanView;
    readonly timeElapsed: TextSpanView;
    readonly taskCount: TextSpanView;
}