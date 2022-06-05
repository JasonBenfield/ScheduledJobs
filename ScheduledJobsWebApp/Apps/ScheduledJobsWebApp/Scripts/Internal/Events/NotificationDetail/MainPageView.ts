import { CardView } from '@jasonbenfield/sharedwebapp/Card/CardView';
import { Block } from '@jasonbenfield/sharedwebapp/Html/Block';
import { Container } from '@jasonbenfield/sharedwebapp/Html/Container';
import { FlexColumn } from '@jasonbenfield/sharedwebapp/Html/FlexColumn';
import { FlexColumnFill } from '@jasonbenfield/sharedwebapp/Html/FlexColumnFill';
import { TextHeading1View } from '@jasonbenfield/sharedwebapp/Html/TextHeading1View';
import { ListGroupView } from '@jasonbenfield/sharedwebapp/ListGroup/ListGroupView';
import { MessageAlertView } from '@jasonbenfield/sharedwebapp/MessageAlertView';
import { PaddingCss } from '@jasonbenfield/sharedwebapp/PaddingCss';
import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { JobSummaryListItemView } from '../../Jobs/JobSummaryListItemView';

export class MainPageView {
    constructor(page: PageFrameView) {
        let flexColumn = page.addContent(new FlexColumn())
            .addContent(new FlexColumnFill());
        this.alert = flexColumn.addContent(new MessageAlertView());
        this.detailBlock = flexColumn.addContent(new Container());
        let eventBlock = this.detailBlock.addContent(new Block());
        this.eventDisplayText = eventBlock.addContent(new TextHeading1View());
        eventBlock.setPadding(PaddingCss.xs({ top: 3, bottom: 3 }));
        let card = this.detailBlock.addContent(new CardView());
        this.triggeredJobsTitle = card.addCardTitleHeader();
        this.triggeredJobs = card.addBlockListGroup(
            () => new JobSummaryListItemView()
        );
    }

    readonly alert: MessageAlertView;
    private readonly detailBlock: Block;
    readonly eventDisplayText: ITextComponentView;
    readonly triggeredJobsTitle: ITextComponentView;
    readonly triggeredJobs: ListGroupView;

    showJobDetail() { this.detailBlock.show(); }

    hideJobDetail() { this.detailBlock.hide(); }
}
