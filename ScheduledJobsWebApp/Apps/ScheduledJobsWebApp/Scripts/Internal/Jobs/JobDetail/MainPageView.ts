import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { MainMenuPanelView } from '../../MainMenuPanelVIew';
import { JobDetailPanelView } from './JobDetailPanelView';
import { TaskDetailPanelView } from './TaskDetailPanelView';

export class MainPageView {
    readonly jobDetailPanel: JobDetailPanelView;
    readonly taskDetailPanel: TaskDetailPanelView;
    readonly menuPanel: MainMenuPanelView;

    constructor(page: PageFrameView) {
        this.jobDetailPanel = page.addContent(new JobDetailPanelView());
        this.taskDetailPanel = page.addContent(new TaskDetailPanelView());
        this.menuPanel = page.addContent(new MainMenuPanelView());
    }
}
