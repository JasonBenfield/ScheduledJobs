import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { MainMenuPanelView } from '../../MainMenuPanelVIew';
import { JobListPanelView } from '../JobListPanelView';

export class MainPageView {
    readonly jobListPanel: JobListPanelView;
    readonly menuPanel: MainMenuPanelView;

    constructor(page: PageFrameView) {
        this.jobListPanel = page.addContent(new JobListPanelView());
        this.menuPanel = page.addContent(new MainMenuPanelView());
    }
}