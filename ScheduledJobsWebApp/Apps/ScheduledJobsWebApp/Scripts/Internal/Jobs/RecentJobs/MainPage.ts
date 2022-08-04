import { BasicPage } from '@jasonbenfield/sharedwebapp/Components/BasicPage';
import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { Apis } from '../../Apis';
import { MainMenuPanel } from '../../MainMenuPanel';
import { MainPageView } from './MainPageView';
import { RecentJobsPanel } from './RecentJobsPanel';

class MainPage extends BasicPage {
    protected readonly view: MainPageView;
    private readonly panels = new SingleActivePanel();
    private readonly jobsPanel: RecentJobsPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor() {
        super(new MainPageView());
        const schdJobsApi = new Apis(this.view.modalError).ScheduledJobs();
        this.jobsPanel = this.panels.add(new RecentJobsPanel(schdJobsApi, this.view.jobsPanel));
        this.menuPanel = this.panels.add(new MainMenuPanel(schdJobsApi, this.view.menuPanel));
        this.jobsPanel.refresh();
        this.activateFailedJobsPanel();
    }

    private async activateFailedJobsPanel() {
        this.panels.activate(this.jobsPanel);
        let result = await this.jobsPanel.start();
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