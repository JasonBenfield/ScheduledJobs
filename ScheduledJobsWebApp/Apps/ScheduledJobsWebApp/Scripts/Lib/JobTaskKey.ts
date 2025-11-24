
export class JobTaskKey {
    readonly value: string;
    readonly displayText: string;

    constructor(source?: IJobTaskKey) {
        this.value = source ? source.Value : "";
        this.displayText = source ? source.DisplayText : "";
    }
}