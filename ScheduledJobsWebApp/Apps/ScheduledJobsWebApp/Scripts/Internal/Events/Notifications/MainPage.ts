import { Startup } from '@jasonbenfield/sharedwebapp/Startup';
import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { MainPageView } from './MainPageView';
import { TextBlock } from '@jasonbenfield/sharedwebapp/Html/TextBlock';
import { ScheduledJobsAppApi } from '../../../ScheduledJobs/Api/ScheduledJobsAppApi';
import { Apis } from '../../Apis';
import { MessageAlert } from '@jasonbenfield/sharedwebapp/MessageAlert';
import { ListGroup } from '@jasonbenfield/sharedwebapp/ListGroup/ListGroup';
import { EventListItem } from './EventSummaryListItem';
import { EventSummaryListItemView } from './EventSummaryListItemView';

class MainPage {
    private readonly alert: MessageAlert;
    private readonly recentEventsList: ListGroup;
    private readonly schdJobsApi: ScheduledJobsAppApi;

    constructor(page: PageFrameView) {
        this.schdJobsApi = new Apis(page.modalError).ScheduledJobs();
        let view = new MainPageView(page);
        this.alert = new MessageAlert(view.alert);
        this.recentEventsList = new ListGroup(view.recentEvents);
        new TextBlock('Events', view.heading);
        this.load();
    }

    private async load() {
        let recentEvents = await this.getRecentEvents();
        this.recentEventsList.setItems(
            recentEvents,
            (evt, itemView: EventSummaryListItemView) => new EventListItem(this.schdJobsApi, evt, itemView)
        );
        if (recentEvents.length === 0) {
            this.alert.danger('No events were found.');
        }
    }

    private async getRecentEvents() {
        let recentEvents: IEventSummaryModel[];
        await this.alert.infoAction(
            'Loading...',
            async () => {
                recentEvents = await this.schdJobsApi.EventInquiry.GetRecentNotifications();
            }
        );
        return recentEvents;
    }
}
new MainPage(new Startup().build());