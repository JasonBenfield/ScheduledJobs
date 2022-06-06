import { ColumnCss } from "@jasonbenfield/sharedwebapp/ColumnCss";
import { ContextualClass } from "@jasonbenfield/sharedwebapp/ContextualClass";
import { Row } from "@jasonbenfield/sharedwebapp/Grid/Row";
import { HtmlSpanView } from "@jasonbenfield/sharedwebapp/Html/HtmlSpanView";
import { TextSpanView } from "@jasonbenfield/sharedwebapp/Html/TextSpanView";
import { ButtonListGroupItemView } from "@jasonbenfield/sharedwebapp/ListGroup/ButtonListGroupItemView";
import { MarginCss } from "@jasonbenfield/sharedwebapp/MarginCss";
import { TextCss } from "@jasonbenfield/sharedwebapp/TextCss";

export class TaskListItemView extends ButtonListGroupItemView {
    constructor() {
        super();
        let row = this.addContent(new Row());
        let col1 = row.addColumn();
        this.generation = col1.addContent(new HtmlSpanView());
        this.displayText = col1.addContent(new TextSpanView());
        let col2 = row.addColumn();
        this.timeStarted = col2
            .addContent(new TextSpanView());
        this.timeStarted.setMargin(MarginCss.end(3));
        this.timeElapsed = col2
            .addContent(new TextSpanView());
        let col3 = row.addColumn();
        col3.setColumnCss(ColumnCss.xs(2));
        this.status = col3.addContent(new TextSpanView());
        let col4 = row.addColumn();
        col4.setColumnCss(ColumnCss.xs(2));
        col4.setTextCss(new TextCss().end());
        this.logEntryCount = col4
            .addContent(new TextSpanView());
        this.logEntryCount.addCssName('badge');
        this.logEntryCount.setBackgroundContext(ContextualClass.secondary);
    }

    readonly generation: HtmlSpanView;
    readonly displayText: ITextComponentView;
    readonly status: ITextComponentView;
    readonly timeStarted: TextSpanView;
    readonly timeElapsed: TextSpanView;
    readonly logEntryCount: TextSpanView;

    failed() {
        this.setContext(ContextualClass.danger);
        this.logEntryCount.setBackgroundContext(ContextualClass.danger);
    }
}