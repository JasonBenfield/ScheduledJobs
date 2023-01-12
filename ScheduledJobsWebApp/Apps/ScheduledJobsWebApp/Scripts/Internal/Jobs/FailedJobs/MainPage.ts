import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { MainMenuPanel } from '../../MainMenuPanel';
import { ScheduledJobsPage } from '../../ScheduledJobsPage';
import { FailedJobsPanel } from './FailedJobsPanel';
import { MainPageView } from './MainPageView';

class MainPage extends ScheduledJobsPage {
    protected readonly view: MainPageView;
    private readonly panels = new SingleActivePanel();
    private readonly failedJobsPanel: FailedJobsPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor() {
        super(new MainPageView());
        this.failedJobsPanel = this.panels.add(new FailedJobsPanel(this.defaultApi, this.view.jobListPanel));
        this.menuPanel = this.panels.add(new MainMenuPanel(this.defaultApi, this.view.menuPanel));
        this.failedJobsPanel.refresh();
        this.activateFailedJobsPanel();
    }

    private async activateFailedJobsPanel() {
        this.panels.activate(this.failedJobsPanel);
        const result = await this.failedJobsPanel.start();
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