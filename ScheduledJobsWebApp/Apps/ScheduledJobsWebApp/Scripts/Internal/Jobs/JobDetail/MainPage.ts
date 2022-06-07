import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { Startup } from '@jasonbenfield/sharedwebapp/Startup';
import { Url } from '@jasonbenfield/sharedwebapp/Url';
import { Apis } from '../../Apis';
import { MainMenuPanel } from '../../MainMenuPanel';
import { JobDetailPanel } from './JobDetailPanel';
import { MainPageView } from './MainPageView';
import { TaskDetailPanel } from './TaskDetailPanel';

class MainPage {
    private readonly panels: SingleActivePanel;
    private readonly jobDetailPanel: JobDetailPanel;
    private readonly taskDetailPanel: TaskDetailPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor(page: PageFrameView) {
        let view = new MainPageView(page);
        let schdJobsApi = new Apis(page.modalError).ScheduledJobs();
        this.panels = new SingleActivePanel();
        this.jobDetailPanel = this.panels.add(new JobDetailPanel(schdJobsApi, view.jobDetailPanel));
        this.taskDetailPanel = this.panels.add(
            new TaskDetailPanel(schdJobsApi, view.taskDetailPanel)
        );
        this.menuPanel = this.panels.add(new MainMenuPanel(schdJobsApi, view.menuPanel));
        this.jobDetailPanel.setJobID(Number(Url.current().getQueryValue('JobID')));
        this.jobDetailPanel.refresh();
        this.activateJobDetailPanel();
    }

    private async activateJobDetailPanel() {
        this.panels.activate(this.jobDetailPanel);
        let result = await this.jobDetailPanel.start();
        if (result.taskSelected) {
            this.taskDetailPanel.setTasks(result.taskSelected.tasks);
            this.taskDetailPanel.setCurrentTask(result.taskSelected.selectedTask);
            this.activateTaskDetailPanel();
        }
        else if (result.menuRequested) {
            this.activateMenuPanel();
        }
    }

    private async activateTaskDetailPanel() {
        this.panels.activate(this.taskDetailPanel);
        let result = await this.taskDetailPanel.start();
        if (result.backRequested) {
            if (result.backRequested.refreshRequired) {
                this.jobDetailPanel.refresh();
            }
            this.activateJobDetailPanel();
        }
    }

    private async activateMenuPanel() {
        this.panels.activate(this.menuPanel);
        let result = await this.menuPanel.start();
        if (result.done) {
            this.activateJobDetailPanel();
        }
    }
}
new MainPage(new Startup().build());