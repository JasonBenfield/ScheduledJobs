import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { Startup } from '@jasonbenfield/sharedwebapp/Startup';
import { Url } from '@jasonbenfield/sharedwebapp/Url';
import { Apis } from '../../Apis';
import { MainMenuPanel } from '../../MainMenuPanel';
import { MainPageView } from './MainPageView';
import { NotificationDetailPanel } from './NotificationDetailPanel';

class MainPage {
    private readonly panels = new SingleActivePanel();
    private readonly notificationDetailPanel: NotificationDetailPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor(page: PageFrameView) {
        let view = new MainPageView(page);
        let schdJobsApi = new Apis(page.modalError).ScheduledJobs();
        this.notificationDetailPanel = this.panels.add(
            new NotificationDetailPanel(schdJobsApi, view.notificationDetailPanel)
        );
        this.menuPanel = this.panels.add(new MainMenuPanel(schdJobsApi, view.menuPanel));
        this.notificationDetailPanel.setNotificationID(
            Number(Url.current().getQueryValue('NotificationID'))
        );
        this.notificationDetailPanel.refresh();
        this.activateNotificationDetailPanel();
    }

    private async activateNotificationDetailPanel() {
        this.panels.activate(this.notificationDetailPanel);
        let result = await this.notificationDetailPanel.start();
        if (result.menuRequested) {
            this.activateMenuPanel();
        }
    }

    private async activateMenuPanel() {
        this.panels.activate(this.menuPanel);
        let result = await this.menuPanel.start();
        if (result.done) {
            this.activateNotificationDetailPanel();
        }
    }
}
new MainPage(new Startup().build());