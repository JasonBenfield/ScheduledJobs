import { TextBlock } from '@jasonbenfield/sharedwebapp/Html/TextBlock';
import { ListGroup } from '@jasonbenfield/sharedwebapp/ListGroup/ListGroup';
import { MessageAlert } from '@jasonbenfield/sharedwebapp/MessageAlert';
import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { Startup } from '@jasonbenfield/sharedwebapp/Startup';
import { Url } from '@jasonbenfield/sharedwebapp/Url';
import { ScheduledJobsAppApi } from '../../../ScheduledJobs/Api/ScheduledJobsAppApi';
import { Apis } from '../../Apis';
import { JobSummaryListItem } from '../../Jobs/JobSummaryListItem';
import { JobSummaryListItemView } from '../../Jobs/JobSummaryListItemView';
import { MainPageView } from './MainPageView';

class MainPage {
    private readonly schdJobsApi: ScheduledJobsAppApi;
    private readonly view: MainPageView;
    private readonly triggeredJobs: ListGroup;
    private readonly alert: MessageAlert;

    constructor(page: PageFrameView) {
        this.view = new MainPageView(page);
        this.schdJobsApi = new Apis(page.modalError).ScheduledJobs();
        this.alert = new MessageAlert(this.view.alert);
        this.view.hideJobDetail();
        new TextBlock('Triggered Jobs', this.view.triggeredJobsTitle);
        this.triggeredJobs = new ListGroup(this.view.triggeredJobs);
        this.load();
    }

    private async load() {
        let jobID = Number(Url.current().getQueryValue('NotificationID'));
        let notificationDetail = await this.getNotificationDetail(jobID);
        new TextBlock(notificationDetail.Event.Definition.EventKey.DisplayText, this.view.eventDisplayText);
        this.triggeredJobs.setItems(
            notificationDetail.TriggeredJobs,
            (job, itemView: JobSummaryListItemView) => new JobSummaryListItem(this.schdJobsApi, job, itemView)
        );
        this.view.showJobDetail();
    }

    private async getNotificationDetail(notificationID: number) {
        let notificationDetail: IEventNotificationDetailModel;
        await this.alert.infoAction(
            'Loading...',
            async () => {
                notificationDetail = await this.schdJobsApi.EventInquiry.GetNotificationDetail(
                    { NotificationID: notificationID }
                );
            }
        );
        return notificationDetail;
    }
}
new MainPage(new Startup().build());