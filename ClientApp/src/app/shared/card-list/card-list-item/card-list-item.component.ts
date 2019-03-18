import { Component } from '@angular/core';
import { Input } from '@angular/core';

@Component({
  selector: 'app-card-list-item',
  templateUrl: './card-list-item.component.html',
  styleUrls: ['./card-list-item.component.css']
})
export class CardListItemComponent {
  private _card: CardListItem;
  private id: string;
  private title: string;
  private subtitle: string;
  private avatar: string;
  private image: string;
  private content: string;
  private actions: CardAction[];

  public get card(): CardListItem {
    return this._card;
  }

  @Input()
  public set card(card: CardListItem) {
    this._card = card;
    this.id = card.id;
    this.title = card.title;
    this.subtitle = card.subtitle;
    this.avatar = card.avatar;
    this.image = card.image;
    this.content = card.content;
    this.actions = card.actions;
  }

  constructor() {}
}

export interface CardListItem {
  id: string;
  title?: string;
  subtitle?: string;
  avatar?: string;
  image?: string;
  content?: any;
  actions?: CardAction[];
}

export interface CardAction {
  label: string;
  isIcon: boolean;
  action: (cardId: string) => void;
}
