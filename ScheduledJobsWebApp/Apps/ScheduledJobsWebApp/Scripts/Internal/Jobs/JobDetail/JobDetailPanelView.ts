import { ColumnCss } from "@jasonbenfield/sharedwebapp/ColumnCss";
import { ButtonCommandItem } from "@jasonbenfield/sharedwebapp/Command/ButtonCommandItem";
import { Block } from "@jasonbenfield/sharedwebapp/Html/Block";
import { Container } from "@jasonbenfield/sharedwebapp/Html/Container";
import { FlexColumn } from "@jasonbenfield/sharedwebapp/Html/FlexColumn";
import { FlexColumnFill } from "@jasonbenfield/sharedwebapp/Html/FlexColumnFill";
import { FormGroupView } from "@jasonbenfield/sharedwebapp/Html/FormGroupView";
import { ListBlockViewModel } from "@jasonbenfield/sharedwebapp/Html/ListBlockViewModel";
import { TextHeading1View } from "@jasonbenfield/sharedwebapp/Html/TextHeading1View";
import { TextLinkView } from "@jasonbenfield/sharedwebapp/Html/TextLinkView";
import { ListGroupView } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroupView";
import { MessageAlertView } from "@jasonbenfield/sharedwebapp/MessageAlertView";
import { PaddingCss } from "@jasonbenfield/sharedwebapp/PaddingCss";
import { ScheduledJobsTheme } from "../../ScheduledJobsTheme";
import { TaskListItemView } from "./TaskListItemView";

export class JobDetailPanelView extends Block {
    readonly alert: MessageAlertView;
    private readonly jobBlock: Container;
    readonly jobDisplayText: ITextComponentView;
    readonly triggeredByFormGroup: FormGroupView;
    readonly triggeredByLink: TextLinkView;
    readonly tasks: ListGroupView;
    readonly menuButton: ButtonCommandItem;
    readonly refreshButton: ButtonCommandItem;

    constructor() {
        super();
        this.height100();
        let flexColumn = this.addContent(new FlexColumn());
        let flexFill = flexColumn.addContent(new FlexColumnFill());
        this.alert = flexFill.addContent(new MessageAlertView());
        this.jobBlock = flexFill.addContent(new Container());
        this.jobDisplayText = this.jobBlock.addContent(new TextHeading1View());
        this.triggeredByFormGroup = this.jobBlock.addContent(new FormGroupView());
        this.triggeredByFormGroup.captionColumn.setColumnCss(ColumnCss.xs('auto'));
        this.triggeredByFormGroup.valueColumn.addCssName('form-control-plaintext');
        this.triggeredByLink = this.triggeredByFormGroup.valueColumn.addContent(new TextLinkView());
        this.jobBlock.setPadding(PaddingCss.xs({ top: 3, bottom: 3 }));
        this.tasks = flexFill.addContent(
            new ListGroupView(
                () => new TaskListItemView(),
                new ListBlockViewModel()
            )
        );
        let toolbar = flexColumn.addContent(ScheduledJobsTheme.instance.commandToolbar.toolbar());
        this.menuButton = toolbar.columnStart.addContent(
            ScheduledJobsTheme.instance.commandToolbar.menuButton()
        );
        this.refreshButton = toolbar.columnStart.addContent(
            ScheduledJobsTheme.instance.commandToolbar.refreshButton()
        );
    }

    showJob() {
        this.jobBlock.show();
        this.tasks.show();
    }

    hideJob() {
        this.jobBlock.hide();
        this.tasks.hide();
    }
}