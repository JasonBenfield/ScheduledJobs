import { Startup } from '@jasonbenfield/sharedwebapp/Startup';
import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { MainPageView } from './MainPageView';
import { TextBlock } from '@jasonbenfield/sharedwebapp/Html/TextBlock';
import { ScheduledJobsAppApi } from '../../../ScheduledJobs/Api/ScheduledJobsAppApi';
import { Apis } from '../../Apis';
import { MessageAlert } from '@jasonbenfield/sharedwebapp/MessageAlert';
import { ListGroup } from '@jasonbenfield/sharedwebapp/ListGroup/ListGroup';
import { JobSummaryListItem } from './JobSummaryListItem';
import { JobSummaryListItemView } from './JobSummaryListItemView';

class MainPage {
    private readonly alert: MessageAlert;
    private readonly failedJobsList: ListGroup;
    private readonly schdJobsApi: ScheduledJobsAppApi;

    constructor(page: PageFrameView) {
        this.schdJobsApi = new Apis(page.modalError).ScheduledJobs();
        let view = new MainPageView(page);
        this.alert = new MessageAlert(view.alert);
        this.failedJobsList = new ListGroup(view.failedJobs);
        new TextBlock('Failed Jobs', view.heading);
        this.load();
    }

    private async load() {
        let failedJobs = await this.getFailedJobs();
        this.failedJobsList.setItems(
            failedJobs,
            (job, itemView: JobSummaryListItemView) => new JobSummaryListItem(this.schdJobsApi, job, itemView)
        );
        if (failedJobs.length === 0) {
            this.alert.success('No failed jobs were found.');
        }
    }

    private async getFailedJobs() {
        let failedJobs: IJobSummaryModel[];
        await this.alert.infoAction(
            'Loading...',
            async () => {
                failedJobs = await this.schdJobsApi.JobInquiry.GetFailedJobs();
            }
        );
        return failedJobs;
    }
}
new MainPage(new Startup().build());