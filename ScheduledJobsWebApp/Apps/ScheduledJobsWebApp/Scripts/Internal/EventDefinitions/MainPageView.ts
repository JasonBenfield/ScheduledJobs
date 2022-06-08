import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';
import { MainMenuPanelView } from '../MainMenuPanelVIew';
import { EventDefinitionListPanelView } from './EventDefinitionListPanelView';

export class MainPageView {
    readonly eventDefinitionsPanel: EventDefinitionListPanelView;
    readonly menuPanel: MainMenuPanelView;

    constructor(page: PageFrameView) {
        this.eventDefinitionsPanel = page.addContent(new EventDefinitionListPanelView());
        this.menuPanel = page.addContent(new MainMenuPanelView());
    }
}