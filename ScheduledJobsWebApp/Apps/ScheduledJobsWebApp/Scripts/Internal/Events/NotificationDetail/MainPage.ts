import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { Url } from '@jasonbenfield/sharedwebapp/Url';
import { MainMenuPanel } from '../../MainMenuPanel';
import { ScheduledJobsPage } from '../../ScheduledJobsPage';
import { MainPageView } from './MainPageView';
import { NotificationDetailPanel } from './NotificationDetailPanel';

class MainPage extends ScheduledJobsPage {
    protected readonly view: MainPageView;
    private readonly panels = new SingleActivePanel();
    private readonly notificationDetailPanel: NotificationDetailPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor() {
        super(new MainPageView());
        this.notificationDetailPanel = this.panels.add(
            new NotificationDetailPanel(this.schdJobsClient, this.view.notificationDetailPanel)
        );
        this.menuPanel = this.panels.add(new MainMenuPanel(this.schdJobsClient, this.view.menuPanel));
        this.notificationDetailPanel.setNotificationID(
            Number(Url.current().getQueryValue('NotificationID'))
        );
        this.notificationDetailPanel.refresh();
        this.activateNotificationDetailPanel();
    }

    private async activateNotificationDetailPanel() {
        this.panels.activate(this.notificationDetailPanel);
        const result = await this.notificationDetailPanel.start();
        if (result.menuRequested) {
            this.activateMenuPanel();
        }
    }

    private async activateMenuPanel() {
        this.panels.activate(this.menuPanel);
        const result = await this.menuPanel.start();
        if (result.done) {
            this.activateNotificationDetailPanel();
        }
    }
}
new MainPage();