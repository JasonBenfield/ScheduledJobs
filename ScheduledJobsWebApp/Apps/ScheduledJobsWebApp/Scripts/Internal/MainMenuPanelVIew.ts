import { CssLengthUnit } from "@jasonbenfield/sharedwebapp/CssLengthUnit";
import { FlexCss } from "@jasonbenfield/sharedwebapp/FlexCss";
import { MarginCss } from "@jasonbenfield/sharedwebapp/MarginCss";
import { BasicComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicComponentView";
import { ButtonCommandView } from "@jasonbenfield/sharedwebapp/Views/Command";
import { GridView } from "@jasonbenfield/sharedwebapp/Views/Grid";
import { NavView } from "@jasonbenfield/sharedwebapp/Views/NavView";
import { TextLinkView } from "@jasonbenfield/sharedwebapp/Views/TextLinkView";
import { ToolbarView } from "@jasonbenfield/sharedwebapp/Views/ToolbarView";
import { ScheduledJobsTheme } from "./ScheduledJobsTheme";

export class MainMenuPanelView extends GridView {
    readonly doneButton: ButtonCommandView;
    readonly eventDefinitionsLink: TextLinkView;
    readonly notificationsLink: TextLinkView;
    readonly jobDefinitionsLink: TextLinkView;
    readonly failedJobsLink: TextLinkView;
    readonly recentJobsLink: TextLinkView;
    private readonly toolbar: ToolbarView;

    constructor(container: BasicComponentView) {
        super(container);
        this.height100();
        this.layout();
        this.setTemplateRows(CssLengthUnit.flex(1), CssLengthUnit.auto());
        let mainContent = ScheduledJobsTheme.instance.mainContent(
            this.addCell()
        );
        let navView = mainContent.addView(NavView);
        navView.pills();
        navView.setFlexCss(new FlexCss().column());
        this.eventDefinitionsLink = navView.addTextLink();
        this.eventDefinitionsLink.setText('Event Definitions');
        this.eventDefinitionsLink.setMargin(MarginCss.bottom(3));
        this.notificationsLink = navView.addTextLink();
        this.notificationsLink.setText('Event Notifications');
        this.notificationsLink.setMargin(MarginCss.bottom(3));
        this.jobDefinitionsLink = navView.addTextLink();
        this.jobDefinitionsLink.setText('Job Definitions');
        this.jobDefinitionsLink.setMargin(MarginCss.bottom(3));
        this.failedJobsLink = navView.addTextLink();
        this.failedJobsLink.setText('Failed Jobs');
        this.failedJobsLink.setMargin(MarginCss.bottom(3));
        this.recentJobsLink = navView.addTextLink();
        this.recentJobsLink.setText('Recent Jobs');
        this.recentJobsLink.setMargin(MarginCss.bottom(3));
        this.toolbar = ScheduledJobsTheme.instance.commandToolbar.toolbar(
            this.addCell().addView(ToolbarView)
        );
        this.doneButton = ScheduledJobsTheme.instance.commandToolbar.backButton(
            this.toolbar.columnStart.addView(ButtonCommandView)
        );
        this.doneButton.setText('Back');
    }

    hideToolbar() { this.toolbar.hide(); }
}