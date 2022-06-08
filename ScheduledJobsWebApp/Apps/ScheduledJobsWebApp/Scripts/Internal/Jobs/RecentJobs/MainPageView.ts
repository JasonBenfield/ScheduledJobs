import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { MainMenuPanelView } from '../../MainMenuPanelVIew';
import { JobListPanelView } from '../JobListPanelView';

export class MainPageView {
    readonly jobsPanel: JobListPanelView;
    readonly menuPanel: MainMenuPanelView;

    constructor(page: PageFrameView) {
        this.jobsPanel = page.addContent(new JobListPanelView());
        this.menuPanel = page.addContent(new MainMenuPanelView());
    }
}