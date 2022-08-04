import { BasicPageView } from '@jasonbenfield/sharedwebapp/Views/BasicPageView';
import { MainMenuPanelView } from '../MainMenuPanelVIew';
import { EventDefinitionListPanelView } from './EventDefinitionListPanelView';
import { NotificationListPanelView } from './NotificationListPanelView';
import { SelectSourceKeyPanelView } from './SelectSourceKeyPanelView';

export class MainPageView extends BasicPageView {
    readonly eventDefinitionsPanel: EventDefinitionListPanelView;
    readonly selectSourceKeyPanel: SelectSourceKeyPanelView;
    readonly notificationsPanel: NotificationListPanelView;
    readonly menuPanel: MainMenuPanelView;

    constructor() {
        super();
        this.eventDefinitionsPanel = this.addView(EventDefinitionListPanelView);
        this.selectSourceKeyPanel = this.addView(SelectSourceKeyPanelView);
        this.notificationsPanel = this.addView(NotificationListPanelView);
        this.menuPanel = this.addView(MainMenuPanelView);
    }
}