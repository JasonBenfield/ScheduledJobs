import { MainMenuPanelView } from '../../MainMenuPanelVIew';
import { ScheduledJobsPageView } from '../../ScheduledJobsPageView';
import { JobListPanelView } from '../JobListPanelView';

export class MainPageView extends ScheduledJobsPageView {
    readonly jobsPanel: JobListPanelView;
    readonly menuPanel: MainMenuPanelView;

    constructor() {
        super();
        this.jobsPanel = this.addView(JobListPanelView);
        this.menuPanel = this.addView(MainMenuPanelView);
    }
}