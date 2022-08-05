import { ContextualClass } from "@jasonbenfield/sharedwebapp/ContextualClass";
import { RowView } from "@jasonbenfield/sharedwebapp/Views/RowView";
import { TextSpanView } from "@jasonbenfield/sharedwebapp/Views/TextSpanView";
import { LinkListGroupItemView } from "@jasonbenfield/sharedwebapp/Views/ListGroup";
import { TextCss } from "@jasonbenfield/sharedwebapp/TextCss";
import { BasicTextComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicTextComponentView";
import { BasicComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicComponentView";

export class EventSummaryListItemView extends LinkListGroupItemView {
    readonly displayText: BasicTextComponentView;
    readonly timeActive: BasicTextComponentView;
    readonly sourceKey: BasicTextComponentView;
    readonly sourceData: BasicTextComponentView;
    readonly triggeredJobCount: BasicTextComponentView;

    constructor(container: BasicComponentView) {
        super(container);
        const row = this.addView(RowView);
        const col1 = row.addColumn();
        this.displayText = col1.addView(TextSpanView);
        const col2 = row.addColumn();
        this.timeActive = col2.addView(TextSpanView);
        const col3 = row.addColumn()
            .configure(c => c.setTextCss(new TextCss().truncate()));
        this.sourceKey = col3.addView(TextSpanView);
        this.sourceData = col3.addView(TextSpanView);
        let col4 = row.addColumn();
        col4.setTextCss(new TextCss().end());
        this.triggeredJobCount = col4
            .addView(TextSpanView)
            .configure(ts => {
                ts.addCssName('badge');
                ts.setBackgroundContext(ContextualClass.secondary);
            });
    }
}