
export class FormattedTimeSpan {
    private readonly text: string;

    constructor(timeStarted: Date, timeEnded: Date) {
        if (timeStarted.getFullYear() > 1 && timeEnded.getFullYear() < 9999) {
            let timeElapsed = timeEnded.getTime() - timeStarted.getTime();
            this.text = `${timeElapsed} ms`;
        }
        else {
            this.text = '';
        }
    }

    format() {
        return this.text;
    }

    toString() {
        return this.format();
    }
}