import { TextHeading1View } from '@jasonbenfield/sharedwebapp/Html/TextHeading1View';
import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';

export class MainPageView {
    readonly heading: ITextComponentView;

    constructor(page: PageFrameView) {
        this.heading = page.addContent(new TextHeading1View());
    }
}