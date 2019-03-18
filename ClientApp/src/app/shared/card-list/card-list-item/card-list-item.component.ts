import { Component } from '@angular/core';
import { Input } from '@angular/core';

@Component({
  selector: 'app-card-list-item',
  templateUrl: './card-list-item.component.html',
  styleUrls: ['./card-list-item.component.css']
})
export class CardListItemComponent {
  @Input() id: string;
  @Input() title: string;
  @Input() subtitle: string;
  @Input() avatar: string;
  @Input() image: string;
  @Input() content: string;
  @Input() actions: CardAction[];

  constructor() {}
}

export interface CardAction {
  label: string;
  isIcon: boolean;
  action: (cardId: string) => void;
}
