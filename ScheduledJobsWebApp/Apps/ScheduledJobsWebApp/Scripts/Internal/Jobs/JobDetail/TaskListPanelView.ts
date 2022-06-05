import { Block } from "@jasonbenfield/sharedwebapp/Html/Block";
import { FlexColumn } from "@jasonbenfield/sharedwebapp/Html/FlexColumn";
import { FlexColumnFill } from "@jasonbenfield/sharedwebapp/Html/FlexColumnFill";
import { ListBlockViewModel } from "@jasonbenfield/sharedwebapp/Html/ListBlockViewModel";
import { ListGroupView } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroupView";
import { TaskListItemView } from "./TaskListItemView";

export class TaskListPanelView extends Block {
    constructor() {
        super();
        this.height100();
        let flexColumn = this.addContent(new FlexColumn());
        let flexFill = flexColumn.addContent(new FlexColumnFill());
        this.tasks = flexFill.addContent(
            new ListGroupView(() => new TaskListItemView(), new ListBlockViewModel())
        );
    }

    readonly tasks: ListGroupView;
}