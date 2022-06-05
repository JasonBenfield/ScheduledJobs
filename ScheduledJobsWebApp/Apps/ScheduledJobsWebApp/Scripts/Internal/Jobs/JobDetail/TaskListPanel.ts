import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { ListGroup } from "@jasonbenfield/sharedwebapp/ListGroup/ListGroup";
import { TaskListItem } from "./TaskListItem";
import { TaskListItemView } from "./TaskListItemView";
import { TaskListPanelView } from "./TaskListPanelView";

interface Results {
    taskSelected?: { task: ITriggeredJobTaskModel };
}

export class TaskListPanelResult {
    static taskSelected(task: ITriggeredJobTaskModel) {
        return new TaskListPanelResult({ taskSelected: { task: task } });
    }

    private constructor(private readonly results: Results) { }

    get taskSelected() { return this.results.taskSelected; }
}

export class TaskListPanel implements IPanel {
    private readonly awaitable = new Awaitable<TaskListPanelResult>();
    private readonly taskList: ListGroup;

    constructor(private readonly view: TaskListPanelView) {
        this.taskList = new ListGroup(this.view.tasks);
        this.taskList.itemClicked.register(this.onTaskClicked.bind(this));
    }

    private onTaskClicked(taskItem: TaskListItem) {
        this.awaitable.resolve(
            TaskListPanelResult.taskSelected(taskItem.task)
        );
    }

    setTasks(tasks: ITriggeredJobTaskModel[]) {
        this.taskList.setItems(
            tasks,
            (task, itemView: TaskListItemView) => new TaskListItem(task, itemView)
        );
    }

    start() {
        return this.awaitable.start();
    }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }

}