import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { MainMenuPanel } from '../../MainMenuPanel';
import { ScheduledJobsPage } from '../../ScheduledJobsPage';
import { MainPageView } from './MainPageView';
import { NotificationListPanel } from './NotificationListPanel';

class MainPage extends ScheduledJobsPage {
    protected readonly view: MainPageView;
    private readonly panels = new SingleActivePanel();
    private readonly notificationListPanel: NotificationListPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor() {
        super(new MainPageView());
        this.notificationListPanel = this.panels.add(new NotificationListPanel(this.schdJobsClient, this.view.notificationListPanel));
        this.menuPanel = this.panels.add(new MainMenuPanel(this.schdJobsClient, this.view.menuPanel));
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