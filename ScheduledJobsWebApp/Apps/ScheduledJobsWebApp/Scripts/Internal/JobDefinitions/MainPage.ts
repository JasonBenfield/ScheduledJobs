import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { Startup } from '@jasonbenfield/sharedwebapp/Startup';
import { Apis } from '../Apis';
import { MainMenuPanel } from '../MainMenuPanel';
import { JobDefinitionListPanel } from './JobDefinitionListPanel';
import { JobListPanel } from './JobListPanel';
import { MainPageView } from './MainPageView';

class MainPage {
    private readonly panels = new SingleActivePanel();
    private readonly jobDefinitionsPanel: JobDefinitionListPanel;
    private readonly jobsPanel: JobListPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor(page: PageFrameView) {
        let schdJobsApi = new Apis(page.modalError).ScheduledJobs();
        let view = new MainPageView(page);
        this.jobDefinitionsPanel = this.panels.add(
            new JobDefinitionListPanel(schdJobsApi, view.jobDefinitionsPanel)
        );
        this.jobsPanel = this.panels.add(
            new JobListPanel(schdJobsApi, view.jobsPanel)
        );
        this.menuPanel = this.panels.add(
            new MainMenuPanel(schdJobsApi, view.menuPanel)
        );
        this.jobDefinitionsPanel.refresh();
        this.activateJobDefinitionsPanel();
    }

    private async activateJobDefinitionsPanel() {
        this.panels.activate(this.jobDefinitionsPanel);
        let result = await this.jobDefinitionsPanel.start();
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
        let result = await this.jobsPanel.start();
        if (result.back) {
            this.activateJobDefinitionsPanel();
        }
    }

    private async activateMenuPanel() {
        this.panels.activate(this.menuPanel);
        let result = await this.menuPanel.start();
        if (result.done) {
            this.activateJobDefinitionsPanel();
        }
    }
}
new MainPage(new Startup().build());