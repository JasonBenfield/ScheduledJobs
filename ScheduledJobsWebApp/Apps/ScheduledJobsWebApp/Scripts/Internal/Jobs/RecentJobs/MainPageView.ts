import { BasicPageView } from '@jasonbenfield/sharedwebapp/Views/BasicPageView';
import { MainMenuPanelView } from '../../MainMenuPanelVIew';
import { JobListPanelView } from '../JobListPanelView';

export class MainPageView extends BasicPageView {
    readonly jobsPanel: JobListPanelView;
    readonly menuPanel: MainMenuPanelView;

    constructor() {
        super();
        this.jobsPanel = this.addView(JobListPanelView);
        this.menuPanel = this.addView(MainMenuPanelView);
    }
}