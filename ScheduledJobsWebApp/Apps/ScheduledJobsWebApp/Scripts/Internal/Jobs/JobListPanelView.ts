﻿import { ButtonCommandItem } from "@jasonbenfield/sharedwebapp/Command/ButtonCommandItem";
import { Block } from "@jasonbenfield/sharedwebapp/Html/Block";
import { FlexColumn } from "@jasonbenfield/sharedwebapp/Html/FlexColumn";
import { FlexColumnFill } from "@jasonbenfield/sharedwebapp/Html/FlexColumnFill";
import { ListBlockViewModel } from "@jasonbenfield/sharedwebapp/Html/ListBlockViewModel";
import { TextHeading1View } from "@jasonbenfield/sharedwebapp/Html/TextHeading1View";
import { ListGroupView } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroupView";
import { MessageAlertView } from "@jasonbenfield/sharedwebapp/MessageAlertView";
import { ScheduledJobsTheme } from "../ScheduledJobsTheme";
import { JobSummaryListItemView } from "./JobSummaryListItemView";

export class JobListPanelView extends Block {
    readonly heading: ITextComponentView;
    readonly alert: MessageAlertView;
    readonly jobs: ListGroupView;
    readonly backButton: ButtonCommandItem;
    readonly menuButton: ButtonCommandItem;
    readonly refreshButton: ButtonCommandItem;

    constructor() {
        super();
        this.height100();
        let flexColumn = this.addContent(new FlexColumn());
        let flexFill = flexColumn.addContent(new FlexColumnFill());
        this.heading = flexFill.addContent(new TextHeading1View());
        this.alert = flexFill.addContent(new MessageAlertView());
        this.jobs = flexFill.addContent(
            new ListGroupView(
                () => new JobSummaryListItemView(),
                new ListBlockViewModel()
            )
        );
        let toolbar = flexColumn.addContent(ScheduledJobsTheme.instance.commandToolbar.toolbar());
        this.backButton = toolbar.columnStart.addContent(
            ScheduledJobsTheme.instance.commandToolbar.backButton()
        );
        this.backButton.hide();
        this.menuButton = toolbar.columnStart.addContent(
            ScheduledJobsTheme.instance.commandToolbar.menuButton()
        );
        this.refreshButton = toolbar.columnStart.addContent(
            ScheduledJobsTheme.instance.commandToolbar.refreshButton()
        );
    }
}