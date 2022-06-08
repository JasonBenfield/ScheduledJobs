import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { MainMenuPanelView } from '../MainMenuPanelVIew';
import { EventDefinitionListPanelView } from './EventDefinitionListPanelView';
import { NotificationListPanelView } from './NotificationListPanelView';
import { SelectSourceKeyPanelView } from './SelectSourceKeyPanelView';

export class MainPageView {
    readonly eventDefinitionsPanel: EventDefinitionListPanelView;
    readonly selectSourceKeyPanel: SelectSourceKeyPanelView;
    readonly notificationsPanel: NotificationListPanelView;
    readonly menuPanel: MainMenuPanelView;

    constructor(page: PageFrameView) {
        this.eventDefinitionsPanel = page.addContent(new EventDefinitionListPanelView());
        this.selectSourceKeyPanel = page.addContent(new SelectSourceKeyPanelView());
        this.notificationsPanel = page.addContent(new NotificationListPanelView());
        this.menuPanel = page.addContent(new MainMenuPanelView());
    }
}