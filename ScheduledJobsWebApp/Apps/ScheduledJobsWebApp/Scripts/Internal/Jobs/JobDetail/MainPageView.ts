import { ColumnCss } from '@jasonbenfield/sharedwebapp/ColumnCss';
import { Block } from '@jasonbenfield/sharedwebapp/Html/Block';
import { Container } from '@jasonbenfield/sharedwebapp/Html/Container';
import { FlexColumn } from '@jasonbenfield/sharedwebapp/Html/FlexColumn';
import { FlexColumnFill } from '@jasonbenfield/sharedwebapp/Html/FlexColumnFill';
import { FormGroupView } from '@jasonbenfield/sharedwebapp/Html/FormGroupView';
import { Link } from '@jasonbenfield/sharedwebapp/Html/Link';
import { LinkView } from '@jasonbenfield/sharedwebapp/Html/LinkView';
import { TextHeading1View } from '@jasonbenfield/sharedwebapp/Html/TextHeading1View';
import { TextLinkView } from '@jasonbenfield/sharedwebapp/Html/TextLinkView';
import { MessageAlertView } from '@jasonbenfield/sharedwebapp/MessageAlertView';
import { PaddingCss } from '@jasonbenfield/sharedwebapp/PaddingCss';
import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { TaskDetailPanelView } from './TaskDetailPanelView';
import { TaskListPanelView } from './TaskListPanelView';

export class MainPageView {
    constructor(page: PageFrameView) {
        let flexColumn = page.addContent(new FlexColumn());
        this.alert = flexColumn.addContent(new MessageAlertView());
        this.jobBlock = flexColumn.addContent(new Container());
        this.jobDisplayText = this.jobBlock.addContent(new TextHeading1View());
        this.triggeredByFormGroup = this.jobBlock.addContent(new FormGroupView());
        this.triggeredByFormGroup.captionColumn.setColumnCss(ColumnCss.xs('auto'));
        this.triggeredByFormGroup.valueColumn.addCssName('form-control-plaintext');
        this.triggeredByLink = this.triggeredByFormGroup.valueColumn.addContent(new TextLinkView());
        this.jobBlock.setPadding(PaddingCss.xs({ top: 3, bottom: 3 }));
        let flexFill = flexColumn.addContent(new FlexColumnFill());
        flexFill.container.height100();
        this.taskListPanel = flexFill.addContent(new TaskListPanelView());
        this.taskListPanel.hide();
        this.taskDetailPanel = flexFill.addContent(new TaskDetailPanelView());
        this.taskDetailPanel.hide();
    }

    readonly alert: MessageAlertView;
    private readonly jobBlock: Container;
    readonly jobDisplayText: ITextComponentView;
    readonly triggeredByFormGroup: FormGroupView;
    readonly triggeredByLink: TextLinkView;
    readonly taskListPanel: TaskListPanelView;
    readonly taskDetailPanel: TaskDetailPanelView;

    showJob() { this.jobBlock.show(); }

    hideJob() { this.jobBlock.hide(); }
}
