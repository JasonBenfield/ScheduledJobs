import { ButtonCommandItem } from "@jasonbenfield/sharedwebapp/Command/ButtonCommandItem";
import { FlexCss } from "@jasonbenfield/sharedwebapp/FlexCss";
import { Block } from "@jasonbenfield/sharedwebapp/Html/Block";
import { FlexColumn } from "@jasonbenfield/sharedwebapp/Html/FlexColumn";
import { FlexColumnFill } from "@jasonbenfield/sharedwebapp/Html/FlexColumnFill";
import { NavLinkView } from "@jasonbenfield/sharedwebapp/Html/NavLinkView";
import { NavView } from "@jasonbenfield/sharedwebapp/Html/NavView";
import { TextSpanView } from "@jasonbenfield/sharedwebapp/Html/TextSpanView";
import { Toolbar } from "@jasonbenfield/sharedwebapp/Html/Toolbar";
import { ScheduledJobsTheme } from "./ScheduledJobsTheme";

export class MainMenuPanelView extends Block {
    readonly doneButton: ButtonCommandItem;
    readonly eventDefinitionsLink: NavLinkView;
    readonly notificationsLink: NavLinkView;
    readonly jobDefinitionsLink: NavLinkView;
    readonly failedJobsLink: NavLinkView;
    readonly recentJobsLink: NavLinkView;
    private readonly toolbar: Toolbar;

    constructor() {
        super();
        this.height100();
        let flexColumn = this.addContent(new FlexColumn());
        let flexFill = flexColumn.addContent(new FlexColumnFill());
        let navView = flexFill.addContent(new NavView());
        navView.pills();
        navView.setFlexCss(new FlexCss().column());
        this.eventDefinitionsLink = navView.addLink();
        this.eventDefinitionsLink.addContent(new TextSpanView())
            .configure(ts => ts.setText('Event Definitions'));
        this.notificationsLink = navView.addLink();
        this.notificationsLink.addContent(new TextSpanView())
            .configure(ts => ts.setText('Event Notifications'));
        this.jobDefinitionsLink = navView.addLink();
        this.jobDefinitionsLink.addContent(new TextSpanView())
            .configure(ts => ts.setText('Job Definitions'));
        this.failedJobsLink = navView.addLink();
        this.failedJobsLink.addContent(new TextSpanView())
            .configure(ts => ts.setText('Failed Jobs'));
        this.recentJobsLink = navView.addLink();
        this.recentJobsLink.addContent(new TextSpanView())
            .configure(ts => ts.setText('Recent Jobs'));
        this.toolbar = flexColumn.addContent(ScheduledJobsTheme.instance.commandToolbar.toolbar());
        this.doneButton = this.toolbar.columnStart.addContent(ScheduledJobsTheme.instance.commandToolbar.backButton());
        this.doneButton.setText('Back');
    }

    hideToolbar() { this.toolbar.hide(); }
}