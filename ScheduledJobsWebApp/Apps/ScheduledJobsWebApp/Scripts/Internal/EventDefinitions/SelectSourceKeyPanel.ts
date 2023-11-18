import { Awaitable } from "@jasonbenfield/sharedwebapp/Awaitable";
import { Command } from "@jasonbenfield/sharedwebapp/Components/Command";
import { TextInputFormGroup } from "@jasonbenfield/sharedwebapp/Forms/TextInputFormGroup";
import { SelectSourceKeyPanelView } from "./SelectSourceKeyPanelView";

interface IResults {
    back?: {};
    next?: { sourceKey: string; }
}

export class SelectSourceKeyPanelResult {
    static back() { return new SelectSourceKeyPanelResult({ back: {} }); }

    static next(sourceKey: string) {
        return new SelectSourceKeyPanelResult({ next: { sourceKey: sourceKey } });
    }

    private constructor(private readonly results: IResults) { }

    get back() { return this.results.back; }

    get next() { return this.results.next; }
}

export class SelectSourceKeyPanel implements IPanel {
    private readonly awaitable = new Awaitable<SelectSourceKeyPanelResult>();
    private readonly sourceKey: TextInputFormGroup;

    constructor(private readonly view: SelectSourceKeyPanelView) {
        this.sourceKey = new TextInputFormGroup('', '', view.sourceKey);
        this.sourceKey.setCaption('Source Key (opt.)');
        new Command(this.back.bind(this)).add(view.backButton);
        new Command(this.next.bind(this)).add(view.nextButton);
        view.handleFormSubmitted(this.onFormSubmitted.bind(this));
    }

    private onFormSubmitted(el: HTMLElement, evt: JQuery.Event) {
        evt.preventDefault();
        this.next();
    }

    private back() { this.awaitable.resolve(SelectSourceKeyPanelResult.back()); }

    private next() {
        this.awaitable.resolve(SelectSourceKeyPanelResult.next(this.sourceKey.getValue()));
    }

    start() {
        return this.awaitable.start();
    }

    activate() {
        this.view.show();
        this.sourceKey.setFocus();
    }

    deactivate() { this.view.hide(); }

}