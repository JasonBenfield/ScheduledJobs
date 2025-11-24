
export class JobKey {
    readonly value: string;
    readonly displayText: string;

    constructor(source?: IJobKey) {
        this.value = source ? source.Value : "";
        this.displayText = source ? source.DisplayText : "";
    }
}