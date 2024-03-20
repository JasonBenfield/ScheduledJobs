import { ColumnCss } from "@jasonbenfield/sharedwebapp/ColumnCss";
import { CssLengthUnit } from "@jasonbenfield/sharedwebapp/CssLengthUnit";
import { BasicComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicComponentView";
import { BasicTextComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicTextComponentView";
import { ButtonCommandView } from "@jasonbenfield/sharedwebapp/Views/Command";
import { GridView } from "@jasonbenfield/sharedwebapp/Views/Grid";
import { LinkListGroupView } from "@jasonbenfield/sharedwebapp/Views/ListGroup";
import { MessageAlertView } from "@jasonbenfield/sharedwebapp/Views/MessageAlertView";
import { RowView } from "@jasonbenfield/sharedwebapp/Views/RowView";
import { TextHeading3View } from "@jasonbenfield/sharedwebapp/Views/TextHeadings";
import { TextSpanView } from "@jasonbenfield/sharedwebapp/Views/TextSpanView";
import { ToolbarView } from "@jasonbenfield/sharedwebapp/Views/ToolbarView";
import { ScheduledJobsTheme } from "../ScheduledJobsTheme";
import { JobSummaryListItemView } from "./JobSummaryListItemView";
import { ContextualClass } from "@jasonbenfield/sharedwebapp/ContextualClass";
import { CardAlertView, CardView } from "@jasonbenfield/sharedwebapp/Views/Card";
import { MarginCss } from "@jasonbenfield/sharedwebapp/MarginCss";

export class JobListPanelView extends GridView {
    readonly titleTextView: BasicTextComponentView;
    readonly countTextView: BasicTextComponentView;
    readonly alert: CardAlertView;
    readonly jobListView: LinkListGroupView<JobSummaryListItemView>;
    readonly backButton: ButtonCommandView;
    readonly menuButton: ButtonCommandView;
    readonly refreshButton: ButtonCommandView;

    constructor(container: BasicComponentView) {
        super(container);
        this.height100();
        this.styleAsLayout();
        this.setTemplateRows(CssLengthUnit.flex(1), CssLengthUnit.auto());
        const mainContent = ScheduledJobsTheme.instance.mainContent(this.addCell());
        const cardView = mainContent.addView(CardView);
        cardView.setMargin(MarginCss.bottom(3));
        const headingRowView = cardView.addCardHeader().addView(RowView)
        const headingCol1 = headingRowView.addColumn();
        this.titleTextView = headingCol1.addView(TextHeading3View);
        const headingCol2 = headingRowView.addColumn();
        headingCol2.setColumnCss(ColumnCss.xs('auto'));
        this.countTextView = headingCol2.addView(TextSpanView);
        this.countTextView.addCssName('badge');
        this.countTextView.addCssName('rounded-corners');
        this.countTextView.setBackgroundContext(ContextualClass.secondary);
        this.alert = cardView.addCardAlert();
        this.jobListView = cardView.addLinkListGroup(JobSummaryListItemView);
        const toolbar = ScheduledJobsTheme.instance.commandToolbar.toolbar(
            this.addCell().addView(ToolbarView)
        );
        this.backButton = ScheduledJobsTheme.instance.commandToolbar.backButton(
            toolbar.columnStart.addView(ButtonCommandView)
        );
        this.backButton.hide();
        this.menuButton = ScheduledJobsTheme.instance.commandToolbar.menuButton(
            toolbar.columnStart.addView(ButtonCommandView)
        );
        this.refreshButton = ScheduledJobsTheme.instance.commandToolbar.refreshButton(
            toolbar.columnStart.addView(ButtonCommandView)
        );
    }
}