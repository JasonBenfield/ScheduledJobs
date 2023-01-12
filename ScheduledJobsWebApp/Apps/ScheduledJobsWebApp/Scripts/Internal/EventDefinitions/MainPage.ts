import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { MainMenuPanel } from '../MainMenuPanel';
import { ScheduledJobsPage } from '../ScheduledJobsPage';
import { EventDefinitionListPanel } from './EventDefinitionListPanel';
import { MainPageView } from './MainPageView';
import { NotificationListPanel } from './NotificationListPanel';
import { SelectSourceKeyPanel } from './SelectSourceKeyPanel';

class MainPage extends ScheduledJobsPage {
    protected readonly view: MainPageView;
    private readonly panels = new SingleActivePanel();
    private readonly eventDefinitionsPanel: EventDefinitionListPanel;
    private readonly selectSourceKeyPanel: SelectSourceKeyPanel;
    private readonly notificationsPanel: NotificationListPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor() {
        super(new MainPageView());
        this.eventDefinitionsPanel = this.panels.add(
            new EventDefinitionListPanel(this.defaultApi, this.view.eventDefinitionsPanel)
        );
        this.selectSourceKeyPanel = this.panels.add(
            new SelectSourceKeyPanel(this.view.selectSourceKeyPanel)
        );
        this.notificationsPanel = this.panels.add(
            new NotificationListPanel(this.defaultApi, this.view.notificationsPanel)
        );
        this.menuPanel = this.panels.add(
            new MainMenuPanel(this.defaultApi, this.view.menuPanel)
        );
        this.eventDefinitionsPanel.refresh();
        this.activateEventDefinitionsPanel();
    }

    private async activateEventDefinitionsPanel() {
        this.panels.activate(this.eventDefinitionsPanel);
        const result = await this.eventDefinitionsPanel.start();
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
        const result = await this.selectSourceKeyPanel.start();
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
        const result = await this.notificationsPanel.start();
        if (result.back) {
            this.activateEventDefinitionsPanel();
        }
    }

    private async activateMenuPanel() {
        this.panels.activate(this.menuPanel);
        const result = await this.menuPanel.start();
        if (result.done) {
            this.activateEventDefinitionsPanel();
        }
    }
}
new MainPage();