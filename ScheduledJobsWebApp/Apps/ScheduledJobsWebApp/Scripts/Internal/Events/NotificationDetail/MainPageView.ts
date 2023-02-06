import { MainMenuPanelView } from '../../MainMenuPanelVIew';
import { ScheduledJobsPageView } from '../../ScheduledJobsPageView';
import { NotificationDetailPanelView } from './NotificationDetailPanelView';

export class MainPageView extends ScheduledJobsPageView {
    constructor() {
        super();
        this.notificationDetailPanel = this.addView(NotificationDetailPanelView);
        this.menuPanel = this.addView(MainMenuPanelView);
    }

    readonly notificationDetailPanel: NotificationDetailPanelView;
    readonly menuPanel: MainMenuPanelView;
}
