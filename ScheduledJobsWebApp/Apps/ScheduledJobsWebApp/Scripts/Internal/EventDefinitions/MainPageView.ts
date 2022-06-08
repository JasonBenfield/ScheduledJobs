import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { MainMenuPanelView } from '../MainMenuPanelVIew';
import { EventDefinitionListPanelView } from './EventDefinitionListPanelView';
import { NotificationListPanelView } from './NotificationListPanelView';

export class MainPageView {
    readonly eventDefinitionsPanel: EventDefinitionListPanelView;
    readonly notificationsPanel: NotificationListPanelView;
    readonly menuPanel: MainMenuPanelView;

    constructor(page: PageFrameView) {
        this.eventDefinitionsPanel = page.addContent(new EventDefinitionListPanelView());
        this.notificationsPanel = page.addContent(new NotificationListPanelView());
        this.menuPanel = page.addContent(new MainMenuPanelView());
    }
}