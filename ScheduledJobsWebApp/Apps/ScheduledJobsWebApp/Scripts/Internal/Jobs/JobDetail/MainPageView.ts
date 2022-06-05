import { Block } from '@jasonbenfield/sharedwebapp/Html/Block';
import { Container } from '@jasonbenfield/sharedwebapp/Html/Container';
import { FlexColumn } from '@jasonbenfield/sharedwebapp/Html/FlexColumn';
import { FlexColumnFill } from '@jasonbenfield/sharedwebapp/Html/FlexColumnFill';
import { TextHeading1View } from '@jasonbenfield/sharedwebapp/Html/TextHeading1View';
import { MessageAlertView } from '@jasonbenfield/sharedwebapp/MessageAlertView';
import { PaddingCss } from '@jasonbenfield/sharedwebapp/PaddingCss';
import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { TaskDetailPanelView } from './TaskDetailPanelView';
import { TaskListPanelView } from './TaskListPanelView';

export class MainPageView {
    constructor(page: PageFrameView) {
        let flexColumn = page.addContent(new FlexColumn());
        let jobBlock = flexColumn.addContent(new Container());
        this.alert = jobBlock.addContent(new MessageAlertView());
        this.jobDisplayText = jobBlock.addContent(new TextHeading1View());
        jobBlock.setPadding(PaddingCss.xs({ top: 3, bottom: 3 }));
        let flexFill = flexColumn.addContent(new FlexColumnFill());
        flexFill.container.height100();
        this.taskListPanel = flexFill.addContent(new TaskListPanelView());
        this.taskListPanel.hide();
        this.taskDetailPanel = flexFill.addContent(new TaskDetailPanelView());
        this.taskDetailPanel.hide();
    }

    readonly alert: MessageAlertView;
    readonly jobDisplayText: ITextComponentView;
    readonly taskListPanel: TaskListPanelView;
    readonly taskDetailPanel: TaskDetailPanelView;
}
