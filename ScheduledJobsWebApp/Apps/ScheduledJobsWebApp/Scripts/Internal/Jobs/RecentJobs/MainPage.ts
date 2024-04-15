import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { MainMenuPanel } from '../../MainMenuPanel';
import { ScheduledJobsPage } from '../../ScheduledJobsPage';
import { MainPageView } from './MainPageView';
import { RecentJobsPanel } from './RecentJobsPanel';

class MainPage extends ScheduledJobsPage {
    protected readonly view: MainPageView;
    private readonly panels = new SingleActivePanel();
    private readonly jobsPanel: RecentJobsPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor() {
        super(new MainPageView());
        this.jobsPanel = this.panels.add(new RecentJobsPanel(this.schdJobsClient, this.view.jobsPanel));
        this.menuPanel = this.panels.add(new MainMenuPanel(this.schdJobsClient, this.view.menuPanel));
        this.jobsPanel.refresh();
        this.activateFailedJobsPanel();
    }

    private async activateFailedJobsPanel() {
        this.panels.activate(this.jobsPanel);
        const result = await this.jobsPanel.start();
        if (result.menuRequested) {
            this.activateMenuPanel();
        }
    }

    private async activateMenuPanel() {
        this.panels.activate(this.menuPanel);
        const result = await this.menuPanel.start();
        if (result.done) {
            this.activateFailedJobsPanel();
        }
    }
}
new MainPage();