import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { SingleActivePanel } from '@jasonbenfield/sharedwebapp/Panel/SingleActivePanel';
import { Startup } from '@jasonbenfield/sharedwebapp/Startup';
import { Apis } from '../Apis';
import { MainMenuPanel } from '../MainMenuPanel';
import { EventDefinitionListPanel } from './EventDefinitionListPanel';
import { MainPageView } from './MainPageView';

class MainPage {
    private readonly panels = new SingleActivePanel();
    private readonly eventDefinitionsPanel: EventDefinitionListPanel;
    private readonly menuPanel: MainMenuPanel;

    constructor(page: PageFrameView) {
        let schdJobsApi = new Apis(page.modalError).ScheduledJobs();
        let view = new MainPageView(page);
        this.eventDefinitionsPanel = this.panels.add(
            new EventDefinitionListPanel(schdJobsApi, view.eventDefinitionsPanel)
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
        }
        else if (result.menuRequested) {
            this.activateMenuPanel();
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