import { ColumnCss } from "@jasonbenfield/sharedwebapp/ColumnCss";
import { ButtonCommandItem } from "@jasonbenfield/sharedwebapp/Command/ButtonCommandItem";
import { InputFormGroupView } from "@jasonbenfield/sharedwebapp/Forms/InputFormGroupView";
import { Block } from "@jasonbenfield/sharedwebapp/Html/Block";
import { FlexColumn } from "@jasonbenfield/sharedwebapp/Html/FlexColumn";
import { FlexColumnFill } from "@jasonbenfield/sharedwebapp/Html/FlexColumnFill";
import { FormView } from "@jasonbenfield/sharedwebapp/Html/FormView";
import { PaddingCss } from "@jasonbenfield/sharedwebapp/PaddingCss";
import { TextCss } from "@jasonbenfield/sharedwebapp/TextCss";
import { ScheduledJobsTheme } from "../ScheduledJobsTheme";

export class SelectSourceKeyPanelView extends Block {
    readonly sourceKey: InputFormGroupView;
    readonly backButton: ButtonCommandItem;
    readonly nextButton: ButtonCommandItem;
    readonly formSubmitted: IEventHandler<any>;

    constructor() {
        super();
        this.height100();
        let flexColumn = this.addContent(new FlexColumn());
        let flexFill = flexColumn.addContent(new FlexColumnFill());
        flexFill.container.setPadding(PaddingCss.top(3));
        let form = flexFill.addContent(new FormView());
        form.addOffscreenSubmit();
        this.formSubmitted = form.submitted;
        this.sourceKey = form.addContent(new InputFormGroupView());
        this.sourceKey.captionColumn.setColumnCss(ColumnCss.xs(4));
        this.sourceKey.captionColumn.setTextCss(new TextCss().end());
        let toolbar = flexColumn.addContent(ScheduledJobsTheme.instance.commandToolbar.toolbar());
        this.backButton = toolbar.columnStart.addContent(
            ScheduledJobsTheme.instance.commandToolbar.backButton()
        );
        this.nextButton = toolbar.columnEnd.addContent(
            ScheduledJobsTheme.instance.commandToolbar.nextButton()
        );
    }
}