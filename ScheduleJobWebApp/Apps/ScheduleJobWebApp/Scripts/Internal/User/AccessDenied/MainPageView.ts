import { Alert } from '@jasonbenfield/sharedwebapp/Alert';
import { ContextualClass } from '@jasonbenfield/sharedwebapp/ContextualClass';
import { Block } from '@jasonbenfield/sharedwebapp/Html/Block';
import { FlexColumn } from '@jasonbenfield/sharedwebapp/Html/FlexColumn';
import { FlexColumnFill } from '@jasonbenfield/sharedwebapp/Html/FlexColumnFill';
import { TextBlockView } from '@jasonbenfield/sharedwebapp/Html/TextBlockView';
import { TextHeading1View } from '@jasonbenfield/sharedwebapp/Html/TextHeading1View';
import { MarginCss } from '@jasonbenfield/sharedwebapp/MarginCss';
import { PageFrameView } from '@jasonbenfield/sharedwebapp/PageFrameView';

export class MainPageView {
    readonly caption: ITextComponentView;
    readonly message: ITextComponentView;

    constructor(private readonly page: PageFrameView) {
        let container = this.page.addContent(new Block());
        container.height100();
        let flexColumn = container.addContent(new FlexColumn());
        let flexFill = flexColumn.addContent(new FlexColumnFill());
        this.caption = flexFill.addContent(new TextHeading1View())
            .configure(h1 => h1.setMargin(MarginCss.top(3).xs({ bottom: 3 })));
        let alert = flexFill.addContent(new Alert())
            .configure(a => a.setMargin(MarginCss.bottom(3)));
        alert.setContext(ContextualClass.danger);
        this.message = alert.addContent(new TextBlockView());
    }
}
