import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { Startup } from '@jasonbenfield/sharedwebapp/Startup';
import { Apis } from '../../Apis';
import { MainMenuPanel } from '../../MainMenuPanel';
import { FailedJobsPanel } from './FailedJobsPanel';
import { MainPageView } from './MainPageView';

class MainPage {
    private readonly panels = new SingleActivePanel();
    private readonly failedJobsPanel: FailedJobsPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor(page: PageFrameView) {
        let schdJobsApi = new Apis(page.modalError).ScheduledJobs();
        let view = new MainPageView(page);
        this.failedJobsPanel = this.panels.add(new FailedJobsPanel(schdJobsApi, view.jobListPanel));
        this.menuPanel = this.panels.add(new MainMenuPanel(schdJobsApi, view.menuPanel));
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
new MainPage(new Startup().build());