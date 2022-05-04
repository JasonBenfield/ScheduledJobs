import { TextBlock } from '@jasonbenfield/sharedwebapp/Html/TextBlock';
import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { Startup } from '@jasonbenfield/sharedwebapp/Startup';
import { MainPageView } from './MainPageView';

declare let serverError: IErrorModel;

class MainPage {
    private readonly view: MainPageView;

    constructor(page: PageFrameView) {
        this.view = new MainPageView(page);
        new TextBlock(serverError.Caption, this.view.caption);
        new TextBlock(serverError.Message, this.view.message);
    }
}
new MainPage(new Startup().build());