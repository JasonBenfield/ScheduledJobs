import { BasicPage } from '@jasonbenfield/sharedwebapp/Components/BasicPage';
import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { Url } from '@jasonbenfield/sharedwebapp/Url';
import { Apis } from '../../Apis';
import { MainMenuPanel } from '../../MainMenuPanel';
import { MainPageView } from './MainPageView';
import { NotificationDetailPanel } from './NotificationDetailPanel';

class MainPage extends BasicPage {
    protected readonly view: MainPageView;
    private readonly panels = new SingleActivePanel();
    private readonly notificationDetailPanel: NotificationDetailPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor() {
        super(new MainPageView());
        const schdJobsApi = new Apis(this.view.modalError).ScheduledJobs();
        this.notificationDetailPanel = this.panels.add(
            new NotificationDetailPanel(schdJobsApi, this.view.notificationDetailPanel)
        );
        this.menuPanel = this.panels.add(new MainMenuPanel(schdJobsApi, this.view.menuPanel));
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