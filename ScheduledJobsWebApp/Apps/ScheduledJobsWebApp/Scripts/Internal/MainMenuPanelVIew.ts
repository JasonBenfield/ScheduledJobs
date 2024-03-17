import { CssLengthUnit } from "@jasonbenfield/sharedwebapp/CssLengthUnit";
import { FlexCss } from "@jasonbenfield/sharedwebapp/FlexCss";
import { MarginCss } from "@jasonbenfield/sharedwebapp/MarginCss";
import { BasicComponentView } from "@jasonbenfield/sharedwebapp/Views/BasicComponentView";
import { ButtonCommandView } from "@jasonbenfield/sharedwebapp/Views/Command";
import { GridView } from "@jasonbenfield/sharedwebapp/Views/Grid";
import { NavView } from "@jasonbenfield/sharedwebapp/Views/NavView";
import { ToolbarView } from "@jasonbenfield/sharedwebapp/Views/ToolbarView";
import { ScheduledJobsTheme } from "./ScheduledJobsTheme";

export class MainMenuPanelView extends GridView {
    readonly doneButton: ButtonCommandView;
    readonly menu: NavView;
    private readonly toolbar: ToolbarView;

    constructor(container: BasicComponentView) {
        super(container);
        this.height100();
        this.styleAsLayout();
        this.setTemplateRows(CssLengthUnit.flex(1), CssLengthUnit.auto());
        let mainContent = ScheduledJobsTheme.instance.mainContent(
            this.addCell()
        );
        this.menu = mainContent.addView(NavView);
        this.menu.pills();
        this.menu.setFlexCss(new FlexCss().column());
        this.menu.configListItem(li => li.setMargin(MarginCss.bottom(3)));
        this.toolbar = ScheduledJobsTheme.instance.commandToolbar.toolbar(
            this.addCell().addView(ToolbarView)
        );
        this.doneButton = ScheduledJobsTheme.instance.commandToolbar.backButton(
            this.toolbar.columnStart.addView(ButtonCommandView)
        );
        this.doneButton.setText('Back');
    }

    hideToolbar() { this.toolbar.hide(); }
}