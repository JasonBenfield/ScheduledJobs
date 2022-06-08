import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { JobListPanelView } from '../Jobs/JobListPanelView';
import { MainMenuPanelView } from '../MainMenuPanelVIew';
import { JobDefinitionListPanelView } from './JobDefinitionListPanelView';

export class MainPageView {
    readonly jobDefinitionsPanel: JobDefinitionListPanelView;
    readonly jobsPanel: JobListPanelView;
    readonly menuPanel: MainMenuPanelView;

    constructor(page: PageFrameView) {
        this.jobDefinitionsPanel = page.addContent(new JobDefinitionListPanelView());
        this.jobsPanel = page.addContent(new JobListPanelView());
        this.menuPanel = page.addContent(new MainMenuPanelView());
    }
}