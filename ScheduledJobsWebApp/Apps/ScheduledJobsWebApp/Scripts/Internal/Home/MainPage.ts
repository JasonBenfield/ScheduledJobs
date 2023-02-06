import { MainMenuPanel } from '../MainMenuPanel';
import { ScheduledJobsPage } from '../ScheduledJobsPage';
import { MainPageView } from './MainPageView';

class MainPage extends ScheduledJobsPage {
    protected readonly view: MainPageView;

    constructor() {
        super(new MainPageView());
        new MainMenuPanel(this.defaultApi, this.view.menuPanel);
    }
}
new MainPage();