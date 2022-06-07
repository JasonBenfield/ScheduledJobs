import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { Startup } from '@jasonbenfield/sharedwebapp/Startup';
import { Apis } from '../../Apis';
import { MainMenuPanel } from '../../MainMenuPanel';
import { MainPageView } from './MainPageView';
import { NotificationListPanel } from './NotificationListPanel';

class MainPage {
    private readonly panels = new SingleActivePanel();
    private readonly notificationListPanel: NotificationListPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor(page: PageFrameView) {
        let schdJobsApi = new Apis(page.modalError).ScheduledJobs();
        let view = new MainPageView(page);
        this.notificationListPanel = this.panels.add(new NotificationListPanel(schdJobsApi, view.notificationListPanel));
        this.menuPanel = this.panels.add(new MainMenuPanel(schdJobsApi, view.menuPanel));
        this.notificationListPanel.refresh();
        this.activateNotificationListPanel();
    }

    private async activateNotificationListPanel() {
        this.panels.activate(this.notificationListPanel);
        let result = await this.notificationListPanel.start();
        if (result.menuRequested) {
            this.activateMenuPanel();
        }
    }

    private async activateMenuPanel() {
        this.panels.activate(this.menuPanel);
        let result = await this.menuPanel.start();
        if (result.done) {
            this.activateNotificationListPanel();
        }
    }
}
new MainPage(new Startup().build());