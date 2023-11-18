import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { Url } from '@jasonbenfield/sharedwebapp/Url';
import { AppClients } from '../../AppClients';
import { MainMenuPanel } from '../../MainMenuPanel';
import { ScheduledJobsPage } from '../../ScheduledJobsPage';
import { EditTaskDataPanel } from './EditTaskDataPanel';
import { JobDetailPanel } from './JobDetailPanel';
import { MainPageView } from './MainPageView';
import { TaskDetailPanel } from './TaskDetailPanel';

class MainPage extends ScheduledJobsPage {
    protected readonly view: MainPageView;
    private readonly panels: SingleActivePanel;
    private readonly jobDetailPanel: JobDetailPanel;
    private readonly taskDetailPanel: TaskDetailPanel;
    private readonly editTaskDataPanel: EditTaskDataPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor() {
        super(new MainPageView());
        this.panels = new SingleActivePanel();
        this.jobDetailPanel = this.panels.add(new JobDetailPanel(this.schdJobsClient, this.view.jobDetailPanel));
        const hubApi = new AppClients(this.view.modalError).Hub();
        this.taskDetailPanel = this.panels.add(
            new TaskDetailPanel(hubApi, this.schdJobsClient, this.view.taskDetailPanel)
        );
        this.editTaskDataPanel = this.panels.add(
            new EditTaskDataPanel(this.schdJobsClient, this.view.editTaskDataPanel)
        );
        this.menuPanel = this.panels.add(new MainMenuPanel(this.schdJobsClient, this.view.menuPanel));
        this.jobDetailPanel.setJobID(Number(Url.current().getQueryValue('JobID')));
        this.jobDetailPanel.refresh();
        this.activateJobDetailPanel();
    }

    private async activateJobDetailPanel() {
        this.panels.activate(this.jobDetailPanel);
        const result = await this.jobDetailPanel.start();
        if (result.taskSelected) {
            this.taskDetailPanel.setTasks(result.taskSelected.tasks, result.taskSelected.sourceLogEntries);
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
        else if (result.editTaskRequested) {
            this.editTaskDataPanel.setTask(result.editTaskRequested.task);
            this.activateEditTaskDataPanel();
        }
    }

    private async activateEditTaskDataPanel() {
        this.panels.activate(this.editTaskDataPanel);
        const result = await this.editTaskDataPanel.start();
        if (result.cancelled) {
            this.activateJobDetailPanel();
        }
        else if (result.saved) {
            this.jobDetailPanel.refresh();
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