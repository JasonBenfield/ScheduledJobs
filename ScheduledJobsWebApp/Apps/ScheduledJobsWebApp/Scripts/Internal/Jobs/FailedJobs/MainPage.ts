import { BasicPage } from '@jasonbenfield/sharedwebapp/Components/BasicPage';
import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { Apis } from '../../Apis';
import { MainMenuPanel } from '../../MainMenuPanel';
import { FailedJobsPanel } from './FailedJobsPanel';
import { MainPageView } from './MainPageView';

class MainPage extends BasicPage {
    protected readonly view: MainPageView;
    private readonly panels = new SingleActivePanel();
    private readonly failedJobsPanel: FailedJobsPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor() {
        super(new MainPageView());
        const schdJobsApi = new Apis(this.view.modalError).ScheduledJobs();
        this.failedJobsPanel = this.panels.add(new FailedJobsPanel(schdJobsApi, this.view.jobListPanel));
        this.menuPanel = this.panels.add(new MainMenuPanel(schdJobsApi, this.view.menuPanel));
        this.failedJobsPanel.refresh();
        this.activateFailedJobsPanel();
    }

    private async activateFailedJobsPanel() {
        this.panels.activate(this.failedJobsPanel);
        let result = await this.failedJobsPanel.start();
        if (result.menuRequested) {
            this.activateMenuPanel();
        }
    }

    private async activateMenuPanel() {
        this.panels.activate(this.menuPanel);
        let result = await this.menuPanel.start();
        if (result.done) {
            this.activateFailedJobsPanel();
        }
    }
}
new MainPage();