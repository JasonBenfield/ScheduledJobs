
export class FormattedTimeSpan {
    private readonly text: string;

    constructor(timeStarted: Date, timeEnded: Date) {
        if (timeStarted.getFullYear() > 1 && timeEnded.getFullYear() < 9999 && timeEnded.getTime() > timeStarted.getTime()) {
            let timeElapsed = timeEnded.getTime() - timeStarted.getTime();
            if (timeElapsed < 1000) {
                this.text = `${timeElapsed} ms`;
            }
            else if (timeElapsed < 60000) {
                this.text = `${timeElapsed / 1000} s`;
            }
            else if (timeElapsed < 3600000) {
                this.text = `${(timeElapsed / 60000).toFixed(2)} m`;
            }
            else if (timeElapsed < 86400000) {
                this.text = `${(timeElapsed / 3600000).toFixed(2)} hr`;
            }
            else {
                this.text = `${(timeElapsed / 86400000).toFixed(2)} d`;
            }
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