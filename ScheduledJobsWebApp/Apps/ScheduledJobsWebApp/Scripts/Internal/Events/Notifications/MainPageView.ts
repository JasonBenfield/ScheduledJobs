import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { MainMenuPanelView } from '../../MainMenuPanelVIew';
import { NotificationListPanelView } from './NotificationListPanelView';

export class MainPageView {
    readonly notificationListPanel: NotificationListPanelView;
    readonly menuPanel: MainMenuPanelView;

    constructor(page: PageFrameView) {
        this.notificationListPanel = page.addContent(new NotificationListPanelView());
        this.menuPanel = page.addContent(new MainMenuPanelView());
    }
}