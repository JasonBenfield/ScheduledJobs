import { Startup } from '@jasonbenfield/sharedwebapp/Startup';
import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { MainPageView } from './MainPageView';
import { TextBlock } from '@jasonbenfield/sharedwebapp/Html/TextBlock';

class MainPage {
    private readonly view: MainPageView;

    constructor(page: PageFrameView) {
        this.view = new MainPageView(page);
        new TextBlock('Home Page', this.view.heading);
    }
}
new MainPage(new Startup().build());