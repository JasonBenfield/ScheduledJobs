import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { Startup } from '@jasonbenfield/sharedwebapp/Startup';
import { Apis } from '../Apis';
import { NotificationListPanel } from './NotificationListPanel';
import { MainMenuPanel } from '../MainMenuPanel';
import { EventDefinitionListPanel } from './EventDefinitionListPanel';
import { MainPageView } from './MainPageView';
import { SelectSourceKeyPanel } from './SelectSourceKeyPanel';

class MainPage {
    private readonly panels = new SingleActivePanel();
    private readonly eventDefinitionsPanel: EventDefinitionListPanel;
    private readonly selectSourceKeyPanel: SelectSourceKeyPanel;
    private readonly notificationsPanel: NotificationListPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor(page: PageFrameView) {
        let schdJobsApi = new Apis(page.modalError).ScheduledJobs();
        let view = new MainPageView(page);
        this.eventDefinitionsPanel = this.panels.add(
            new EventDefinitionListPanel(schdJobsApi, view.eventDefinitionsPanel)
        );
        this.selectSourceKeyPanel = this.panels.add(
            new SelectSourceKeyPanel(view.selectSourceKeyPanel)
        );
        this.notificationsPanel = this.panels.add(
            new NotificationListPanel(schdJobsApi, view.notificationsPanel)
        );
        this.menuPanel = this.panels.add(
            new MainMenuPanel(schdJobsApi, view.menuPanel)
        );
        this.eventDefinitionsPanel.refresh();
        this.activateEventDefinitionsPanel();
    }

    private async activateEventDefinitionsPanel() {
        this.panels.activate(this.eventDefinitionsPanel);
        let result = await this.eventDefinitionsPanel.start();
        if (result.eventDefinitionSelected) {
            this.notificationsPanel.setEventDefinitionID(result.eventDefinitionSelected.eventDefinitionID);
            this.activateSelectSourceKeyPanel();
        }
        else if (result.menuRequested) {
            this.activateMenuPanel();
        }
    }

    private async activateSelectSourceKeyPanel() {
        this.panels.activate(this.selectSourceKeyPanel);
        let result = await this.selectSourceKeyPanel.start();
        if (result.back) {
            this.activateEventDefinitionsPanel();
        }
        else if (result.next) {
            this.notificationsPanel.setSourceKey(result.next.sourceKey);
            this.notificationsPanel.refresh();
            this.activateNotificationsPanel();
        }
    }

    private async activateNotificationsPanel() {
        this.panels.activate(this.notificationsPanel);
        let result = await this.notificationsPanel.start();
        if (result.back) {
            this.activateEventDefinitionsPanel();
        }
    }

    private async activateMenuPanel() {
        this.panels.activate(this.menuPanel);
        let result = await this.menuPanel.start();
        if (result.done) {
            this.activateEventDefinitionsPanel();
        }
    }
}
new MainPage(new Startup().build());