import { EventKey } from "./EventKey";

export class EventDefinition {
    readonly id: number;
    readonly eventKey: EventKey;

    constructor(source?: IEventDefinitionModel) {
        this.id = source ? source.ID : 0;
        this.eventKey = new EventKey(source && source.EventKey);
    }
}