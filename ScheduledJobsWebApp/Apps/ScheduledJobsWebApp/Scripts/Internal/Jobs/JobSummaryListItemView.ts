import { ContextualClass } from "@jasonbenfield/sharedwebapp/ContextualClass";
import { Row } from "@jasonbenfield/sharedwebapp/Grid/Row";
import { TextSpanView } from "@jasonbenfield/sharedwebapp/Html/TextSpanView";
import { LinkListGroupItemView } from "@jasonbenfield/sharedwebapp/ListGroup/LinkListGroupItemView";
import { LinkListItemViewModel } from "@jasonbenfield/sharedwebapp/ListGroup/LinkListItemViewModel";
import { TextCss } from "@jasonbenfield/sharedwebapp/TextCss";

export class JobSummaryListItemView extends LinkListGroupItemView {

    constructor() {
        super(new LinkListItemViewModel());
        let row = this.addContent(new Row());
        let col1 = row.addColumn();
        this.displayText = col1
            .addContent(new TextSpanView());
        let col2 = row.addColumn();
        this.timeStarted = col2
            .addContent(new TextSpanView());
        this.timeElapsed = col2
            .addContent(new TextSpanView());
        let col3 = row.addColumn();
        this.status = col3
            .addContent(new TextSpanView());
        let col4 = row.addColumn();
        col4.setTextCss(new TextCss().end());
        this.taskCount = col4
            .addContent(new TextSpanView());
        this.taskCount.addCssName('badge');
        this.taskCount.setBackgroundContext(ContextualClass.secondary);
    }

    readonly displayText: TextSpanView;
    readonly status: TextSpanView;
    readonly timeStarted: TextSpanView;
    readonly timeElapsed: TextSpanView;
    readonly taskCount: TextSpanView;
}