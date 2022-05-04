import { ButtonCommandItem } from "@jasonbenfield/sharedwebapp/Command/ButtonCommandItem";
import { ContextualClass } from "@jasonbenfield/sharedwebapp/ContextualClass";
import { Toolbar } from "@jasonbenfield/sharedwebapp/Html/Toolbar";
import { PaddingCss } from "@jasonbenfield/sharedwebapp/PaddingCss";
import { TextCss } from "@jasonbenfield/sharedwebapp/TextCss";

export class ScheduleJobTheme {
    public static readonly instance = new ScheduleJobTheme();

    readonly listItem = {
        deleteButton() {
            return new ButtonCommandItem()
                .configure(b => {
                    b.icon.setName('times');
                    b.icon.addCssFrom(new TextCss().context(ContextualClass.danger).cssClass());
                    b.useOutlineStyle();
                    b.setText('');
                    b.setTitle('Delete');
                });
        }
    }

    readonly cardHeader = {
        editButton() {
            return new ButtonCommandItem()
                .configure(b => {
                    b.icon.setName('edit');
                    b.setContext(ContextualClass.primary);
                    b.useOutlineStyle();
                    b.setText('Edit');
                    b.setTitle('Edit');
                });
        },
        addButton() {
            return new ButtonCommandItem()
                .configure(b => {
                    b.icon.setName('plus');
                    b.setContext(ContextualClass.primary);
                    b.useOutlineStyle();
                    b.setText('Add');
                    b.setTitle('Add');
                });
        }
    }

    readonly commandToolbar = {
        toolbar() {
            return new Toolbar()
                .configure(t => {
                    t.setBackgroundContext(ContextualClass.secondary);
                    t.setPadding(PaddingCss.xs(3));
                });
        },
        backButton() {
            return new ButtonCommandItem()
                .configure(b => {
                    b.icon.setName('caret-left');
                    b.setText('Back');
                    b.setTitle('Back');
                    b.setContext(ContextualClass.light);
                    b.useOutlineStyle();
                });
        },
        cancelButton() {
            return new ButtonCommandItem()
                .configure(b => {
                    b.icon.setName('times');
                    b.setText('Cancel');
                    b.setTitle('Cancel');
                    b.setContext(ContextualClass.danger);
                });
        },
        saveButton() {
            return new ButtonCommandItem()
                .configure(b => {
                    b.icon.setName('check');
                    b.setText('Save');
                    b.setTitle('Save');
                    b.setContext(ContextualClass.primary);
                });
        }
    };
}