import { JobListPanelView } from '../Jobs/JobListPanelView';
import { MainMenuPanelView } from '../MainMenuPanelVIew';
import { ScheduledJobsPageView } from '../ScheduledJobsPageView';
import { JobDefinitionListPanelView } from './JobDefinitionListPanelView';

export class MainPageView extends ScheduledJobsPageView {
    readonly jobDefinitionsPanel: JobDefinitionListPanelView;
    readonly jobsPanel: JobListPanelView;
    readonly menuPanel: MainMenuPanelView;

    constructor() {
        super();
        this.jobDefinitionsPanel = this.addView(JobDefinitionListPanelView);
        this.jobsPanel = this.addView(JobListPanelView);
        this.menuPanel = this.addView(MainMenuPanelView);
    }
}