import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { Startup } from '@jasonbenfield/sharedwebapp/Startup';
import { Apis } from '../Apis';
import { MainMenuPanel } from '../MainMenuPanel';
import { MainPageView } from './MainPageView';

class MainPage {
    constructor(page: PageFrameView) {
        let view = new MainPageView(page);
        let schdJobsApi = new Apis(page.modalError).ScheduledJobs();
        new MainMenuPanel(schdJobsApi, view.menuPanel);
    }
}
new MainPage(new Startup().build());