import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { MainMenuPanelView } from '../../MainMenuPanelVIew';
import { NotificationDetailPanelView } from './NotificationDetailPanelView';

export class MainPageView {
    constructor(page: PageFrameView) {
        this.notificationDetailPanel = page.addContent(new NotificationDetailPanelView());
        this.menuPanel = page.addContent(new MainMenuPanelView());
    }

    readonly notificationDetailPanel: NotificationDetailPanelView;
    readonly menuPanel: MainMenuPanelView;
}
