import { MainMenuPanelView } from '../../MainMenuPanelVIew';
import { ScheduledJobsPageView } from '../../ScheduledJobsPageView';
import { JobListPanelView } from '../JobListPanelView';

export class MainPageView extends ScheduledJobsPageView {
    readonly jobListPanel: JobListPanelView;
    readonly menuPanel: MainMenuPanelView;

    constructor() {
        super();
        this.jobListPanel = this.addView(JobListPanelView);
        this.menuPanel = this.addView(MainMenuPanelView);
    }
}