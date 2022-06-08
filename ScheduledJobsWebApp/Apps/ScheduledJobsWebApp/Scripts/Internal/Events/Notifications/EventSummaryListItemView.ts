import { ContextualClass } from "@jasonbenfield/sharedwebapp/ContextualClass";
import { Row } from "@jasonbenfield/sharedwebapp/Grid/Row";
import { TextSpanView } from "@jasonbenfield/sharedwebapp/Html/TextSpanView";
import { LinkListGroupItemView } from "@jasonbenfield/sharedwebapp/ListGroup/LinkListGroupItemView";
import { LinkListItemViewModel } from "@jasonbenfield/sharedwebapp/ListGroup/LinkListItemViewModel";
import { TextCss } from "@jasonbenfield/sharedwebapp/TextCss";

export class EventSummaryListItemView extends LinkListGroupItemView {
    readonly displayText: ITextComponentView;
    readonly timeActive: ITextComponentView;
    readonly sourceKey: ITextComponentView;
    readonly sourceData: ITextComponentView;
    readonly triggeredJobCount: ITextComponentView;

    constructor() {
        super(new LinkListItemViewModel());
        let row = this.addContent(new Row());
        let col1 = row.addColumn();
        this.displayText = col1
            .addContent(new TextSpanView());
        let col2 = row.addColumn();
        this.timeActive = col2
            .addContent(new TextSpanView());
        let col3 = row.addColumn()
            .configure(c => c.setTextCss(new TextCss().truncate()));
        this.sourceKey = col3.addContent(new TextSpanView());
        this.sourceData = col3.addContent(new TextSpanView());
        let col4 = row.addColumn();
        col4.setTextCss(new TextCss().end());
        this.triggeredJobCount = col4
            .addContent(new TextSpanView())
            .configure(ts => {
                ts.addCssName('badge');
                ts.setBackgroundContext(ContextualClass.secondary);
            });
    }
}