import { Component } from '@angular/core';
import { Input } from '@angular/core';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent {
  private _card: CardDetails;
  private id: string;
  private title: string;
  private subtitle: string;
  private avatar: string;
  private image: string;
  private content: string;
  private actions: CardAction[];
  private linkTo: string[];
  private queryParams: object;

  public get card(): CardDetails {
    return this._card;
  }

  @Input()
  public set card(card: CardDetails) {
    this._card = card;
    this.id = card.id + '';
    this.title = card.title;
    this.subtitle = card.subtitle;
    this.avatar = card.avatar;
    this.image = card.image;
    this.content = card.content;
    this.actions = card.actions;
    this.linkTo = Array.isArray(card.linkTo)
      ? card.linkTo // copy if it's an array
      : typeof card.linkTo === 'string' && [card.linkTo]; // wrap in array if string
    this.queryParams = card.queryParams;
  }

  constructor() {}

  private handleAction($event: Event, action: CardAction) {
    $event.stopPropagation();
    action.action(this.id);
  }
}

export type CardActionHandler = (cardId: string) => void;

export interface CardDetails {
  id: string | number;

  title?: string;
  subtitle?: string;
  avatar?: string;
  image?: string;
  content?: any;
  actions?: CardAction[];
  linkTo?: string[] | string;
  queryParams?: object;
}

export interface CardAction {
  label: string;
  isIcon: boolean;
  action: CardActionHandler;
}
