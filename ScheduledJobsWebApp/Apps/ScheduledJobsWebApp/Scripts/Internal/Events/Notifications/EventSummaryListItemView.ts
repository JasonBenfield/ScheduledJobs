import { ContextualClass } from "@jasonbenfield/sharedwebapp/ContextualClass";
import { Row } from "@jasonbenfield/sharedwebapp/Grid/Row";
import { TextSpanView } from "@jasonbenfield/sharedwebapp/Html/TextSpanView";
import { LinkListGroupItemView } from "@jasonbenfield/sharedwebapp/ListGroup/LinkListGroupItemView";
import { LinkListItemViewModel } from "@jasonbenfield/sharedwebapp/ListGroup/LinkListItemViewModel";
import { TextCss } from "@jasonbenfield/sharedwebapp/TextCss";

export class EventSummaryListItemView extends LinkListGroupItemView {

    constructor() {
        super(new LinkListItemViewModel());
        let row = this.addContent(new Row());
        let col1 = row.addColumn();
        this.displayText = col1
            .addContent(new TextSpanView());
        let col2 = row.addColumn();
        this.timeActive = col2
            .addContent(new TextSpanView());
        let col3 = row.addColumn();
        col3.setTextCss(new TextCss().end());
        this.triggeredJobCount = col3
            .addContent(new TextSpanView());
        this.triggeredJobCount.addCssName('badge');
        this.triggeredJobCount.setBackgroundContext(ContextualClass.secondary);
    }

    readonly displayText: TextSpanView;
    readonly timeActive: TextSpanView;
    readonly triggeredJobCount: TextSpanView;
}