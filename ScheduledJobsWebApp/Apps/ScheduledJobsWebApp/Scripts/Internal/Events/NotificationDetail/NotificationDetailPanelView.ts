﻿import { CssLengthUnit } from "@jasonbenfield/sharedwebapp/CssLengthUnit";
import { MarginCss } from "@jasonbenfield/sharedwebapp/MarginCss";
import { TextCss } from "@jasonbenfield/sharedwebapp/TextCss";
import { BasicComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicComponentView";
import { BasicTextComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicTextComponentView";
import { BlockView } from "@jasonbenfield/sharedwebapp/Views/BlockView";
import { CardView } from "@jasonbenfield/sharedwebapp/Views/Card";
import { ButtonCommandView } from "@jasonbenfield/sharedwebapp/Views/Command";
import { FormGroupTextView } from "@jasonbenfield/sharedwebapp/Views/FormGroup";
import { GridView } from "@jasonbenfield/sharedwebapp/Views/Grid";
import { LinkListGroupView } from "@jasonbenfield/sharedwebapp/Views/ListGroup";
import { MessageAlertView } from "@jasonbenfield/sharedwebapp/Views/MessageAlertView";
import { TextHeading1View } from "@jasonbenfield/sharedwebapp/Views/TextHeadings";
import { ToolbarView } from "@jasonbenfield/sharedwebapp/Views/ToolbarView";
import { JobSummaryListItemView } from "../../Jobs/JobSummaryListItemView";
import { ScheduledJobsTheme } from "../../ScheduledJobsTheme";
import { FormGroupContainerView } from "@jasonbenfield/sharedwebapp/Views/FormGroupContainerView";

export class NotificationDetailPanelView extends GridView {
    readonly alert: MessageAlertView;
    private readonly detailBlock: BlockView;
    readonly eventDisplayText: BasicTextComponentView;
    readonly sourceKey: FormGroupTextView;
    readonly sourceData: FormGroupTextView;
    readonly triggeredJobsTitle: BasicTextComponentView;
    readonly triggeredJobListView: LinkListGroupView<JobSummaryListItemView>;
    readonly menuButton: ButtonCommandView;
    readonly refreshButton: ButtonCommandView;

    constructor(container: BasicComponentView) {
        super(container);
        this.height100();
        this.styleAsLayout();
        this.setTemplateRows(CssLengthUnit.flex(1), CssLengthUnit.auto());
        const mainContent = ScheduledJobsTheme.instance.mainContent(this.addCell());
        this.alert = mainContent.addView(MessageAlertView);
        this.detailBlock = mainContent.addView(BlockView);
        this.eventDisplayText = this.detailBlock.addView(TextHeading1View)
            .configure(th => th.setMargin(MarginCss.bottom(3)));
        const formGroupContainer = this.detailBlock.addView(FormGroupContainerView)
        this.sourceKey = formGroupContainer.addFormGroupTextView();
        this.sourceKey.captionCell.setTextCss(new TextCss().end());
        this.sourceData = formGroupContainer.addFormGroupTextView();
        const card = this.detailBlock.addView(CardView);
        card.setMargin(MarginCss.bottom(3));
        this.triggeredJobsTitle = card.addCardTitleHeader();
        this.triggeredJobListView = card.addLinkListGroup(JobSummaryListItemView);
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

    showJobDetail() { this.detailBlock.show(); }

    hideJobDetail() { this.detailBlock.hide(); }
}