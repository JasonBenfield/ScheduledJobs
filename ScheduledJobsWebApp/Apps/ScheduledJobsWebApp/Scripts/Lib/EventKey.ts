
export class EventKey {
    readonly value: string;
    readonly displayText: string;

    constructor(source?: IEventKey) {
        this.value = source ? source.Value : "";
        this.displayText = source ? source.DisplayText : "";
    }
}