import { ColumnCss } from "@jasonbenfield/sharedwebapp/ColumnCss";
import { TextCss } from "@jasonbenfield/sharedwebapp/TextCss";
import { BasicComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicComponentView";
import { BasicTextComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicTextComponentView";
import { ListGroupItemView } from "@jasonbenfield/sharedwebapp/Views/ListGroup";
import { RowView } from "@jasonbenfield/sharedwebapp/Views/RowView";
import { TextBlockView } from "@jasonbenfield/sharedwebapp/Views/TextBlockView";
import { TextLinkView } from "@jasonbenfield/sharedwebapp/Views/TextLinkView";
import { TextSpanView } from "@jasonbenfield/sharedwebapp/Views/TextSpanView";

export class LogEntryItemView extends ListGroupItemView {
    constructor(container: BasicComponentView) {
        super(container);
        const row1 = this.addView(RowView);
        const col1 = row1.addColumn();
        col1.setColumnCss(ColumnCss.xs(2));
        col1.setTextCss(new TextCss().truncate());
        this.category = col1.addView(TextSpanView);
        const col2 = row1.addColumn();
        this.sourceMessage = col2.addView(TextBlockView);
        this.message = col2.addView(TextBlockView);
        this.details = col2.addView(TextBlockView);
        const row2 = this.addView(RowView);
        const linkCol = row2.addColumn();
        this.sourceLogEntryLink = linkCol.addView(TextLinkView);
        this.sourceLogEntryLink.setText('View Source Log Entry');
    }

    readonly category: BasicTextComponentView;
    readonly sourceMessage: BasicTextComponentView;
    readonly message: BasicTextComponentView;
    readonly details: BasicTextComponentView;
    readonly sourceLogEntryLink: TextLinkView;
}