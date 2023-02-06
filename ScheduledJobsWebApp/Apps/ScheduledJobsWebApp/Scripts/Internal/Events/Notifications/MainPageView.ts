import { MainMenuPanelView } from '../../MainMenuPanelVIew';
import { ScheduledJobsPageView } from '../../ScheduledJobsPageView';
import { NotificationListPanelView } from './NotificationListPanelView';

export class MainPageView extends ScheduledJobsPageView {
    readonly notificationListPanel: NotificationListPanelView;
    readonly menuPanel: MainMenuPanelView;

    constructor() {
        super();
        this.notificationListPanel = this.addView(NotificationListPanelView);
        this.menuPanel = this.addView(MainMenuPanelView);
    }
}