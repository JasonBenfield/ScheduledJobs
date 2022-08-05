import { BasicPageView } from '@jasonbenfield/sharedwebapp/Views/BasicPageView';
import { MainMenuPanelView } from '../../MainMenuPanelVIew';
import { NotificationDetailPanelView } from './NotificationDetailPanelView';

export class MainPageView extends BasicPageView {
    constructor() {
        super();
        this.notificationDetailPanel = this.addView(NotificationDetailPanelView);
        this.menuPanel = this.addView(MainMenuPanelView);
    }

    readonly notificationDetailPanel: NotificationDetailPanelView;
    readonly menuPanel: MainMenuPanelView;
}
