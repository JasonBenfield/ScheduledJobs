import { CssLengthUnit } from "@jasonbenfield/sharedwebapp/CssLengthUnit";
import { MarginCss } from "@jasonbenfield/sharedwebapp/MarginCss";
import { BasicComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicComponentView";
import { CardAlertView, CardView } from "@jasonbenfield/sharedwebapp/Views/Card";
import { ButtonCommandView } from "@jasonbenfield/sharedwebapp/Views/Command";
import { GridView } from "@jasonbenfield/sharedwebapp/Views/Grid";
import { ButtonListGroupView } from "@jasonbenfield/sharedwebapp/Views/ListGroup";
import { ToolbarView } from "@jasonbenfield/sharedwebapp/Views/ToolbarView";
import { ScheduledJobsTheme } from "../ScheduledJobsTheme";
import { EventDefinitionListItemView } from "./EventDefinitionListItemView";
import { TextHeading3View } from "@jasonbenfield/sharedwebapp/Views/TextHeadings";

export class EventDefinitionListPanelView extends GridView {
    readonly alert: CardAlertView;
    readonly eventDefinitionListView: ButtonListGroupView<EventDefinitionListItemView>;
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
        const titleView = cardView.addCardHeader().addView(TextHeading3View);
        titleView.addCssName('card-title');
        titleView.setText('Event Definitions');
        this.alert = cardView.addCardAlert();
        this.eventDefinitionListView = cardView.addButtonListGroup(EventDefinitionListItemView);
        const toolbar = ScheduledJobsTheme.instance.commandToolbar.toolbar(
            this.addCell().addView(ToolbarView)
        );
        this.menuButton = ScheduledJobsTheme.instance.commandToolbar.menuButton(
            toolbar.columnStart.addView(ButtonCommandView)
        );
        this.refreshButton = ScheduledJobsTheme.instance.commandToolbar.refreshButton(
            toolbar.columnStart.addView(ButtonCommandView)
        );
    }
}