import { MainMenuPanelView } from '../MainMenuPanelVIew';
import { ScheduledJobsPageView } from '../ScheduledJobsPageView';

export class MainPageView extends ScheduledJobsPageView {
    readonly menuPanel: MainMenuPanelView;

    constructor() {
        super();
        this.menuPanel = this.addView(MainMenuPanelView);
        this.menuPanel.hideToolbar();
    }
}