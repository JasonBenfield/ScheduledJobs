import { CardView } from "@jasonbenfield/sharedwebapp/Card/CardView";
import { ButtonCommandItem } from "@jasonbenfield/sharedwebapp/Command/ButtonCommandItem";
import { Block } from "@jasonbenfield/sharedwebapp/Html/Block";
import { Container } from "@jasonbenfield/sharedwebapp/Html/Container";
import { FlexColumn } from "@jasonbenfield/sharedwebapp/Html/FlexColumn";
import { FlexColumnFill } from "@jasonbenfield/sharedwebapp/Html/FlexColumnFill";
import { TextHeading1View } from "@jasonbenfield/sharedwebapp/Html/TextHeading1View";
import { ListGroupView } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroupView";
import { MessageAlertView } from "@jasonbenfield/sharedwebapp/MessageAlertView";
import { PaddingCss } from "@jasonbenfield/sharedwebapp/PaddingCss";
import { JobSummaryListItemView } from "../../Jobs/JobSummaryListItemView";
import { ScheduledJobsTheme } from "../../ScheduledJobsTheme";

export class NotificationDetailPanelView extends Block {
    readonly alert: MessageAlertView;
    private readonly detailBlock: Block;
    readonly eventDisplayText: ITextComponentView;
    readonly triggeredJobsTitle: ITextComponentView;
    readonly triggeredJobs: ListGroupView;
    readonly menuButton: ButtonCommandItem;
    readonly refreshButton: ButtonCommandItem;

    constructor() {
        super();
        this.height100();
        let flexColumn = this.addContent(new FlexColumn());
        let flexFill = flexColumn.addContent(new FlexColumnFill());
        this.alert = flexFill.addContent(new MessageAlertView());
        this.detailBlock = flexFill.addContent(new Container());
        let eventBlock = this.detailBlock.addContent(new Block());
        this.eventDisplayText = eventBlock.addContent(new TextHeading1View());
        eventBlock.setPadding(PaddingCss.xs({ top: 3, bottom: 3 }));
        let card = this.detailBlock.addContent(new CardView());
        this.triggeredJobsTitle = card.addCardTitleHeader();
        this.triggeredJobs = card.addBlockListGroup(
            () => new JobSummaryListItemView()
        );
        let toolbar = flexColumn.addContent(ScheduledJobsTheme.instance.commandToolbar.toolbar());
        this.menuButton = toolbar.columnStart.addContent(
            ScheduledJobsTheme.instance.commandToolbar.menuButton()
        );
        this.refreshButton = toolbar.columnStart.addContent(
            ScheduledJobsTheme.instance.commandToolbar.refreshButton()
        );
    }

    showJobDetail() { this.detailBlock.show(); }

    hideJobDetail() { this.detailBlock.hide(); }
}