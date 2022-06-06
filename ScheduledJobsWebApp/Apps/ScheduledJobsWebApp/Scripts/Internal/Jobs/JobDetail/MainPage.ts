import { FormGroup } from '@jasonbenfield/sharedwebapp/Html/FormGroup';
import { Link } from '@jasonbenfield/sharedwebapp/Html/Link';
import { TextBlock } from '@jasonbenfield/sharedwebapp/Html/TextBlock';
import { TextLink } from '@jasonbenfield/sharedwebapp/Html/TextLink';
import { MessageAlert } from '@jasonbenfield/sharedwebapp/MessageAlert';
import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { Startup } from '@jasonbenfield/sharedwebapp/Startup';
import { Url } from '@jasonbenfield/sharedwebapp/Url';
import { ScheduledJobsAppApi } from '../../../ScheduledJobs/Api/ScheduledJobsAppApi';
import { Apis } from '../../Apis';
import { MainPageView } from './MainPageView';
import { TaskDetailPanel } from './TaskDetailPanel';
import { TaskListPanel } from './TaskListPanel';

class MainPage {
    private readonly schdJobsApi: ScheduledJobsAppApi;
    private readonly view: MainPageView;
    private readonly panels: SingleActivePanel;
    private readonly taskListPanel: TaskListPanel;
    private readonly taskDetailPanel: TaskDetailPanel;
    private readonly alert: MessageAlert;
    private readonly jobDisplayText: TextBlock;
    private readonly triggeredByLink: TextLink;

    constructor(page: PageFrameView) {
        this.view = new MainPageView(page);
        this.view.hideJob();
        this.schdJobsApi = new Apis(page.modalError).ScheduledJobs();
        this.alert = new MessageAlert(this.view.alert);
        this.jobDisplayText = new TextBlock('', this.view.jobDisplayText);
        new FormGroup(this.view.triggeredByFormGroup).setCaption('Triggered By');
        this.triggeredByLink = new TextLink('', this.view.triggeredByLink);
        this.panels = new SingleActivePanel();
        this.taskListPanel = this.panels.add(new TaskListPanel(this.view.taskListPanel));
        this.taskDetailPanel = this.panels.add(
            new TaskDetailPanel(this.schdJobsApi, this.view.taskDetailPanel)
        );
        this.load();
    }

    private async load() {
        let jobID = Number(Url.current().getQueryValue('JobID'));
        let jobDetail = await this.getJobDetail(jobID);
        this.jobDisplayText.setText(jobDetail.Job.JobDefinition.JobKey.DisplayText);
        this.triggeredByLink.setText(jobDetail.TriggeredBy.Definition.EventKey.DisplayText);
        this.triggeredByLink.setHref(
            this.schdJobsApi.EventInquiry.NotificationDetail.getUrl({
                NotificationID: jobDetail.TriggeredBy.ID
            }).value()
        );
        this.taskListPanel.setTasks(jobDetail.Tasks);
        this.taskDetailPanel.setTasks(jobDetail.Tasks);
        this.activateTaskListPanel();
        this.view.showJob();
    }

    private async getJobDetail(jobID) {
        let jobDetail: ITriggeredJobDetailModel;
        await this.alert.infoAction(
            'Loading...',
            async () => {
                jobDetail = await this.schdJobsApi.JobInquiry.GetJobDetail({ JobID: jobID });
            }
        );
        return jobDetail;
    }

    private async activateTaskListPanel() {
        this.panels.activate(this.taskListPanel);
        let result = await this.taskListPanel.start();
        if (result.taskSelected) {
            this.taskDetailPanel.setCurrentTask(result.taskSelected.task);
            this.activateTaskDetailPanel();
        }
    }

    private async activateTaskDetailPanel() {
        this.panels.activate(this.taskDetailPanel);
        let result = await this.taskDetailPanel.start();
        if (result.backRequested) {
            if (result.backRequested.refreshRequired) {
                this.load();
            }
            else {
                this.activateTaskListPanel();
            }
        }
    }
}
new MainPage(new Startup().build());