import { TextBlock } from '@jasonbenfield/sharedwebapp/Html/TextBlock';
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

    constructor(page: PageFrameView) {
        this.view = new MainPageView(page);
        this.schdJobsApi = new Apis(page.modalError).ScheduledJobs();
        this.alert = new MessageAlert(this.view.alert);
        this.panels = new SingleActivePanel();
        this.taskListPanel = this.panels.add(new TaskListPanel(this.view.taskListPanel));
        this.taskDetailPanel = this.panels.add(new TaskDetailPanel(this.view.taskDetailPanel));
        this.load();
    }

    private async load() {
        let jobID = Number(Url.current().getQueryValue('JobID'));
        let jobDetail = await this.getJobDetail(jobID);
        new TextBlock(jobDetail.Job.JobDefinition.JobKey.DisplayText, this.view.jobDisplayText);
        this.taskListPanel.setTasks(jobDetail.Tasks);
        this.taskDetailPanel.setTasks(jobDetail.Tasks);
        this.activateTaskListPanel();
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
            this.activateTaskListPanel();
        }
    }
}
new MainPage(new Startup().build());