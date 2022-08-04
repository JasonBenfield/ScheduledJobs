import { BasicPage } from '@jasonbenfield/sharedwebapp/Components/BasicPage';
import { Apis } from '../Apis';
import { MainMenuPanel } from '../MainMenuPanel';
import { MainPageView } from './MainPageView';

class MainPage extends BasicPage {
    protected readonly view: MainPageView;

    constructor() {
        super(new MainPageView());
        const schdJobsApi = new Apis(this.view.modalError).ScheduledJobs();
        new MainMenuPanel(schdJobsApi, this.view.menuPanel);
    }
}
new MainPage();