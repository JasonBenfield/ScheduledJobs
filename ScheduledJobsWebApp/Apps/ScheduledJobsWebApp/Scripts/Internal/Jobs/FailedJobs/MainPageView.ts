import { BasicPageView } from '@jasonbenfield/sharedwebapp/Views/BasicPageView';
import { MainMenuPanelView } from '../../MainMenuPanelVIew';
import { JobListPanelView } from '../JobListPanelView';

export class MainPageView extends BasicPageView {
    readonly jobListPanel: JobListPanelView;
    readonly menuPanel: MainMenuPanelView;

    constructor() {
        super();
        this.jobListPanel = this.addView(JobListPanelView);
        this.menuPanel = this.addView(MainMenuPanelView);
    }
}