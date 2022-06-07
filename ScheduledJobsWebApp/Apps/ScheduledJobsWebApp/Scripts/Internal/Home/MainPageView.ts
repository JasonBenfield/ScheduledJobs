import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { MainMenuPanelView } from '../MainMenuPanelVIew';

export class MainPageView {
    readonly menuPanel: MainMenuPanelView;

    constructor(page: PageFrameView) {
        this.menuPanel = page.addContent(new MainMenuPanelView());
        this.menuPanel.hideToolbar();
    }
}