import { BasicPage } from '@jasonbenfield/sharedwebapp/Components/BasicPage';
import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { Apis } from '../Apis';
import { MainMenuPanel } from '../MainMenuPanel';
import { JobDefinitionListPanel } from './JobDefinitionListPanel';
import { JobListPanel } from './JobListPanel';
import { MainPageView } from './MainPageView';

class MainPage extends BasicPage {
    protected readonly view: MainPageView;
    private readonly panels = new SingleActivePanel();
    private readonly jobDefinitionsPanel: JobDefinitionListPanel;
    private readonly jobsPanel: JobListPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor() {
        super(new MainPageView());
        const schdJobsApi = new Apis(this.view.modalError).ScheduledJobs();
        this.jobDefinitionsPanel = this.panels.add(
            new JobDefinitionListPanel(schdJobsApi, this.view.jobDefinitionsPanel)
        );
        this.jobsPanel = this.panels.add(
            new JobListPanel(schdJobsApi, this.view.jobsPanel)
        );
        this.menuPanel = this.panels.add(
            new MainMenuPanel(schdJobsApi, this.view.menuPanel)
        );
        this.jobDefinitionsPanel.refresh();
        this.activateJobDefinitionsPanel();
    }

    private async activateJobDefinitionsPanel() {
        this.panels.activate(this.jobDefinitionsPanel);
        const result = await this.jobDefinitionsPanel.start();
        if (result.jobDefinitionSelected) {
            this.jobsPanel.setJobDefinitionID(result.jobDefinitionSelected.jobDefinitionID);
            this.jobsPanel.refresh();
            this.activateJobsPanel();
        }
        else if (result.menuRequested) {
            this.activateMenuPanel();
        }
    }

    private async activateJobsPanel() {
        this.panels.activate(this.jobsPanel);
        const result = await this.jobsPanel.start();
        if (result.back) {
            this.activateJobDefinitionsPanel();
        }
    }

    private async activateMenuPanel() {
        this.panels.activate(this.menuPanel);
        const result = await this.menuPanel.start();
        if (result.done) {
            this.activateJobDefinitionsPanel();
        }
    }
}
new MainPage();