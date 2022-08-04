import { ColumnCss } from "@jasonbenfield/sharedwebapp/ColumnCss";
import { TextCss } from "@jasonbenfield/sharedwebapp/TextCss";
import { BasicComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicComponentView";
import { BasicTextComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicTextComponentView";
import { ListGroupItemView } from "@jasonbenfield/sharedwebapp/Views/ListGroup";
import { RowView } from "@jasonbenfield/sharedwebapp/Views/RowView";
import { TextBlockView } from "@jasonbenfield/sharedwebapp/Views/TextBlockView";
import { TextSpanView } from "@jasonbenfield/sharedwebapp/Views/TextSpanView";

export class LogEntryItemView extends ListGroupItemView {
    constructor(container: BasicComponentView) {
        super(container);
        const row = this.addView(RowView);
        const col1 = row.addColumn();
        col1.setColumnCss(ColumnCss.xs(2));
        col1.setTextCss(new TextCss().truncate());
        this.category = col1.addView(TextSpanView);
        const col2 = row.addColumn();
        this.message = col2.addView(TextBlockView);
        this.details = col2.addView(TextBlockView);
    }

    readonly category: BasicTextComponentView;
    readonly message: BasicTextComponentView;
    readonly details: BasicTextComponentView;
}