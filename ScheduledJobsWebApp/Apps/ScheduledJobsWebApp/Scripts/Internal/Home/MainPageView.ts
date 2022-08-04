import { BasicPageView } from '@jasonbenfield/sharedwebapp/Views/BasicPageView';
import { MainMenuPanelView } from '../MainMenuPanelVIew';

export class MainPageView extends BasicPageView {
    readonly menuPanel: MainMenuPanelView;

    constructor() {
        super();
        this.menuPanel = this.addView(MainMenuPanelView);
        this.menuPanel.hideToolbar();
    }
}