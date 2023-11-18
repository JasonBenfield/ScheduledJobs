import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { AsyncCommand, Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { MessageAlert } from "@jasonbenfield/sharedwebapp/Components/MessageAlert";
import { TextAreaControl } from "@jasonbenfield/sharedwebapp/Components/TextAreaControl";
import { ScheduledJobsAppClient } from "../../../Lib/Http/ScheduledJobsAppClient";
import { EditTaskDataPanelView } from "./EditTaskDataPanelView";

interface IResult {
    cancelled?: {};
    saved?: {};
}

class Result {
    static cancelled() { return new Result({ cancelled: {} }); }

    static saved() { return new Result({ saved: {} }); }

    private constructor(private readonly results: IResult) { }

    get cancelled() { return this.results.cancelled; }

    get saved() { return this.results.saved; }
}

export class EditTaskDataPanel implements IPanel {
    private readonly awaitable = new Awaitable<Result>();
    private readonly alert: MessageAlert;
    private readonly taskData: TextAreaControl;
    private taskID: number;

    constructor(private readonly schdJobsClient: ScheduledJobsAppClient, private readonly view: EditTaskDataPanelView) {
        this.alert = new MessageAlert(view.alert);
        this.taskData = new TextAreaControl(view.taskDataFormGroup.textArea);
        new Command(this.cancel.bind(this)).add(view.cancelButton);
        new AsyncCommand(this.save.bind(this)).add(view.saveButton);
    }

    private cancel() { this.awaitable.resolve(Result.cancelled()); }

    private async save() {
        await this.alert.infoAction(
            'Saving...',
            () => this.schdJobsClient.Tasks.EditTaskData({
                TaskID: this.taskID,
                TaskData: this.taskData.getValue()
            })
        );
        this.awaitable.resolve(Result.saved());
    }

    setTask(task: ITriggeredJobTaskModel) {
        this.taskID = task.ID;
        this.taskData.setValue(task.TaskData);
    }

    start() { return this.awaitable.start(); }

    activate() { this.view.show(); }

    deactivate() { this.view.hide(); }
}