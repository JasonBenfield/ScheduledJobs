import { ColumnCss } from "@jasonbenfield/sharedwebapp/ColumnCss";
import { Row } from "@jasonbenfield/sharedwebapp/Grid/Row";
import { TextBlockView } from "@jasonbenfield/sharedwebapp/Html/TextBlockView";
import { TextSpanView } from "@jasonbenfield/sharedwebapp/Html/TextSpanView";
import { ListGroupItemView } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroupItemView";
import { TextCss } from "@jasonbenfield/sharedwebapp/TextCss";

export class LogEntryItemView extends ListGroupItemView {
    constructor() {
        super();
        let row = this.addContent(new Row());
        let col1 = row.addColumn();
        col1.setColumnCss(ColumnCss.xs(2));
        this.category = col1.addContent(new TextSpanView());
        let col2 = row.addColumn();
        this.message = col2.addContent(new TextBlockView());
        this.details = col2.addContent(new TextBlockView());
    }

    readonly category: ITextComponentView;
    readonly message: ITextComponentView;
    readonly details: ITextComponentView;
}