import { CssLengthUnit } from "@jasonbenfield/sharedwebapp/CssLengthUnit";
import { BasicComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicComponentView";
import { ButtonCommandView } from "@jasonbenfield/sharedwebapp/Views/Command";
import { GridView } from "@jasonbenfield/sharedwebapp/Views/Grid";
import { LinkListGroupView } from "@jasonbenfield/sharedwebapp/Views/ListGroup";
import { MessageAlertView } from "@jasonbenfield/sharedwebapp/Views/MessageAlertView";
import { ToolbarView } from "@jasonbenfield/sharedwebapp/Views/ToolbarView";
import { EventSummaryListItemView } from "../Events/Notifications/EventSummaryListItemView";
import { ScheduledJobsTheme } from "../ScheduledJobsTheme";
import { MarginCss } from "@jasonbenfield/sharedwebapp/MarginCss";

export class NotificationListPanelView extends GridView {
    readonly alert: MessageAlertView;
    readonly notificationListView: LinkListGroupView<EventSummaryListItemView>;
    readonly backButton: ButtonCommandView;
    readonly refreshButton: ButtonCommandView;

    constructor(container: BasicComponentView) {
        super(container);
        this.height100();
        this.styleAsLayout();
        this.setTemplateRows(CssLengthUnit.flex(1), CssLengthUnit.auto());
        const mainContent = ScheduledJobsTheme.instance.mainContent(this.addCell());
        this.alert = mainContent.addView(MessageAlertView);
        this.notificationListView = mainContent.addLinkListGroup(EventSummaryListItemView);
        this.notificationListView.setMargin(MarginCss.bottom(3));
        const toolbar = ScheduledJobsTheme.instance.commandToolbar.toolbar(
            this.addCell().addView(ToolbarView)
        );
        this.backButton = ScheduledJobsTheme.instance.commandToolbar.backButton(
            toolbar.columnStart.addView(ButtonCommandView)
        );
        this.refreshButton = ScheduledJobsTheme.instance.commandToolbar.refreshButton(
            toolbar.columnStart.addView(ButtonCommandView)
        );
    }
}