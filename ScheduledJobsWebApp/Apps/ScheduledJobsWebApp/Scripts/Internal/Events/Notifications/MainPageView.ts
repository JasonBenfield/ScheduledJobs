import { FlexColumn } from '@jasonbenfield/sharedwebapp/Html/FlexColumn';
import { FlexColumnFill } from '@jasonbenfield/sharedwebapp/Html/FlexColumnFill';
import { ListBlockViewModel } from '@jasonbenfield/sharedwebapp/Html/ListBlockViewModel';
import { TextHeading1View } from '@jasonbenfield/sharedwebapp/Html/TextHeading1View';
import { ListGroupView } from '@jasonbenfield/sharedwebapp/ListGroup/ListGroupView';
import { MessageAlertView } from '@jasonbenfield/sharedwebapp/MessageAlertView';
import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { EventSummaryListItemView } from './EventSummaryListItemView';

export class MainPageView {
    readonly heading: ITextComponentView;
    readonly alert: MessageAlertView;
    readonly recentEvents: ListGroupView;

    constructor(page: PageFrameView) {
        let flexColumn = page.addContent(new FlexColumn())
            .addContent(new FlexColumnFill());
        this.heading = flexColumn.addContent(new TextHeading1View());
        this.alert = flexColumn.addContent(new MessageAlertView());
        this.recentEvents = flexColumn.addContent(
            new ListGroupView(
                () => new EventSummaryListItemView(),
                new ListBlockViewModel()
            )
        );
    }
}