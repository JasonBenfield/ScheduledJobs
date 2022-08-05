import { BasicPage } from '@jasonbenfield/sharedwebapp/Components/BasicPage';
import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { Url } from '@jasonbenfield/sharedwebapp/Url';
import { Apis } from '../../Apis';
import { MainMenuPanel } from '../../MainMenuPanel';
import { JobDetailPanel } from './JobDetailPanel';
import { MainPageView } from './MainPageView';
import { TaskDetailPanel } from './TaskDetailPanel';

class MainPage extends BasicPage {
    protected readonly view: MainPageView;
    private readonly panels: SingleActivePanel;
    private readonly jobDetailPanel: JobDetailPanel;
    private readonly taskDetailPanel: TaskDetailPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor() {
        super(new MainPageView());
        const schdJobsApi = new Apis(this.view.modalError).ScheduledJobs();
        this.panels = new SingleActivePanel();
        this.jobDetailPanel = this.panels.add(new JobDetailPanel(schdJobsApi, this.view.jobDetailPanel));
        this.taskDetailPanel = this.panels.add(
            new TaskDetailPanel(schdJobsApi, this.view.taskDetailPanel)
        );
        this.menuPanel = this.panels.add(new MainMenuPanel(schdJobsApi, this.view.menuPanel));
        this.jobDetailPanel.setJobID(Number(Url.current().getQueryValue('JobID')));
        this.jobDetailPanel.refresh();
        this.activateJobDetailPanel();
    }

    private async activateJobDetailPanel() {
        this.panels.activate(this.jobDetailPanel);
        const result = await this.jobDetailPanel.start();
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
        const result = await this.taskDetailPanel.start();
        if (result.backRequested) {
            if (result.backRequested.refreshRequired) {
                this.jobDetailPanel.refresh();
            }
            this.activateJobDetailPanel();
        }
    }

    private async activateMenuPanel() {
        this.panels.activate(this.menuPanel);
        const result = await this.menuPanel.start();
        if (result.done) {
            this.activateJobDetailPanel();
        }
    }
}
new MainPage();