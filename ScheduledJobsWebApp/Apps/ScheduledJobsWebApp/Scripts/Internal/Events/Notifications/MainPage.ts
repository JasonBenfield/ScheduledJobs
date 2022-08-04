import { BasicPage } from '@jasonbenfield/sharedwebapp/Components/BasicPage';
import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { Apis } from '../../Apis';
import { MainMenuPanel } from '../../MainMenuPanel';
import { MainPageView } from './MainPageView';
import { NotificationListPanel } from './NotificationListPanel';

class MainPage extends BasicPage {
    protected readonly view: MainPageView;
    private readonly panels = new SingleActivePanel();
    private readonly notificationListPanel: NotificationListPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor() {
        super(new MainPageView());
        const schdJobsApi = new Apis(this.view.modalError).ScheduledJobs();
        this.notificationListPanel = this.panels.add(new NotificationListPanel(schdJobsApi, this.view.notificationListPanel));
        this.menuPanel = this.panels.add(new MainMenuPanel(schdJobsApi, this.view.menuPanel));
        this.notificationListPanel.refresh();
        this.activateNotificationListPanel();
    }

    private async activateNotificationListPanel() {
        this.panels.activate(this.notificationListPanel);
        const result = await this.notificationListPanel.start();
        if (result.menuRequested) {
            this.activateMenuPanel();
        }
    }

    private async activateMenuPanel() {
        this.panels.activate(this.menuPanel);
        const result = await this.menuPanel.start();
        if (result.done) {
            this.activateNotificationListPanel();
        }
    }
}
new MainPage();