import { BasicPageView } from '@jasonbenfield/sharedwebapp/Views/BasicPageView';
import { MainMenuPanelView } from '../../MainMenuPanelVIew';
import { EditTaskDataPanelView } from './EditTaskDataPanelView';
import { JobDetailPanelView } from './JobDetailPanelView';
import { TaskDetailPanelView } from './TaskDetailPanelView';

export class MainPageView extends BasicPageView {
    readonly jobDetailPanel: JobDetailPanelView;
    readonly taskDetailPanel: TaskDetailPanelView;
    readonly editTaskDataPanel: EditTaskDataPanelView;
    readonly menuPanel: MainMenuPanelView;

    constructor() {
        super();
        this.jobDetailPanel = this.addView(JobDetailPanelView);
        this.taskDetailPanel = this.addView(TaskDetailPanelView);
        this.editTaskDataPanel = this.addView(EditTaskDataPanelView);
        this.menuPanel = this.addView(MainMenuPanelView);
    }
}
