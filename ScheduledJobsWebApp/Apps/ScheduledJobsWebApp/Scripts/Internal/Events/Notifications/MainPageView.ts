import { BasicPageView } from '@jasonbenfield/sharedwebapp/Views/BasicPageView';
import { MainMenuPanelView } from '../../MainMenuPanelVIew';
import { NotificationListPanelView } from './NotificationListPanelView';

export class MainPageView extends BasicPageView {
    readonly notificationListPanel: NotificationListPanelView;
    readonly menuPanel: MainMenuPanelView;

    constructor() {
        super();
        this.notificationListPanel = this.addView(NotificationListPanelView);
        this.menuPanel = this.addView(MainMenuPanelView);
    }
}