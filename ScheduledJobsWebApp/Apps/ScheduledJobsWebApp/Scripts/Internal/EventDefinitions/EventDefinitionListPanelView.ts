import { ButtonCommandItem } from "@jasonbenfield/sharedwebapp/Command/ButtonCommandItem";
import { Block } from "@jasonbenfield/sharedwebapp/Html/Block";
import { FlexColumn } from "@jasonbenfield/sharedwebapp/Html/FlexColumn";
import { FlexColumnFill } from "@jasonbenfield/sharedwebapp/Html/FlexColumnFill";
import { ListBlockViewModel } from "@jasonbenfield/sharedwebapp/Html/ListBlockViewModel";
import { ListGroupView } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroupView";
import { MessageAlertView } from "@jasonbenfield/sharedwebapp/MessageAlertView";
import { PaddingCss } from "@jasonbenfield/sharedwebapp/PaddingCss";
import { ScheduledJobsTheme } from "../ScheduledJobsTheme";
import { EventDefinitionListItemView } from "./EventDefinitionListItemView";

export class EventDefinitionListPanelView extends Block {
    readonly alert: MessageAlertView;
    readonly eventDefinitions: ListGroupView;
    readonly menuButton: ButtonCommandItem;
    readonly refreshButton: ButtonCommandItem;

    constructor() {
        super();
        this.height100();
        let flexColumn = this.addContent(new FlexColumn());
        let flexFill = flexColumn.addContent(new FlexColumnFill());
        flexFill.container.setPadding(PaddingCss.top(3));
        this.alert = flexFill.addContent(new MessageAlertView());
        this.eventDefinitions = flexFill.addContent(
            new ListGroupView(() => new EventDefinitionListItemView(), new ListBlockViewModel())
        );
        let toolbar = flexColumn.addContent(ScheduledJobsTheme.instance.commandToolbar.toolbar());
        this.menuButton = toolbar.columnStart.addContent(
            ScheduledJobsTheme.instance.commandToolbar.menuButton()
        );
        this.refreshButton = toolbar.columnStart.addContent(
            ScheduledJobsTheme.instance.commandToolbar.refreshButton()
        );
    }
}