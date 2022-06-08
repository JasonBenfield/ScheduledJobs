import { CardView } from "@jasonbenfield/sharedwebapp/Card/CardView";
import { ColumnCss } from "@jasonbenfield/sharedwebapp/ColumnCss";
import { ButtonCommandItem } from "@jasonbenfield/sharedwebapp/Command/ButtonCommandItem";
import { Block } from "@jasonbenfield/sharedwebapp/Html/Block";
import { Container } from "@jasonbenfield/sharedwebapp/Html/Container";
import { FlexColumn } from "@jasonbenfield/sharedwebapp/Html/FlexColumn";
import { FlexColumnFill } from "@jasonbenfield/sharedwebapp/Html/FlexColumnFill";
import { PlainTextFormGroupView } from "@jasonbenfield/sharedwebapp/Html/PlainTextFormGroupView";
import { TextBlockView } from "@jasonbenfield/sharedwebapp/Html/TextBlockView";
import { TextHeading1View } from "@jasonbenfield/sharedwebapp/Html/TextHeading1View";
import { TextValueFormGroupView } from "@jasonbenfield/sharedwebapp/Html/TextValueFormGroupView";
import { ListGroupView } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroupView";
import { MarginCss } from "@jasonbenfield/sharedwebapp/MarginCss";
import { MessageAlertView } from "@jasonbenfield/sharedwebapp/MessageAlertView";
import { PaddingCss } from "@jasonbenfield/sharedwebapp/PaddingCss";
import { TextCss } from "@jasonbenfield/sharedwebapp/TextCss";
import { JobSummaryListItemView } from "../../Jobs/JobSummaryListItemView";
import { ScheduledJobsTheme } from "../../ScheduledJobsTheme";

export class NotificationDetailPanelView extends Block {
    readonly alert: MessageAlertView;
    private readonly detailBlock: Block;
    readonly eventDisplayText: ITextComponentView;
    readonly sourceKey: TextValueFormGroupView;
    readonly sourceData: TextValueFormGroupView;
    readonly triggeredJobsTitle: ITextComponentView;
    readonly triggeredJobs: ListGroupView;
    readonly menuButton: ButtonCommandItem;
    readonly refreshButton: ButtonCommandItem;

    constructor() {
        super();
        this.height100();
        let flexColumn = this.addContent(new FlexColumn());
        let flexFill = flexColumn.addContent(new FlexColumnFill());
        flexFill.container.setPadding(PaddingCss.top(3));
        this.alert = flexFill.addContent(new MessageAlertView());
        this.detailBlock = flexFill.addContent(new Block());
        this.eventDisplayText = this.detailBlock.addContent(new TextHeading1View())
            .configure(th => th.setMargin(MarginCss.bottom(3)));
        this.sourceKey = this.detailBlock.addContent(new TextValueFormGroupView())
            .configure(b => b.setMargin(MarginCss.bottom(3)));
        this.sourceKey.captionColumn.setColumnCss(ColumnCss.xs(3));
        this.sourceKey.captionColumn.setTextCss(new TextCss().end());
        this.sourceData = this.detailBlock.addContent(new TextValueFormGroupView())
            .configure(b => b.setMargin(MarginCss.bottom(3)));
        this.sourceData.captionColumn.setColumnCss(ColumnCss.xs(3));
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