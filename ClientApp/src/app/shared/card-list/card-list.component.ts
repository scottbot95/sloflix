import { Component, OnInit, Input } from '@angular/core';
import { CardListItem } from './card-list-item.interface';

@Component({
  selector: 'app-card-list',
  templateUrl: './card-list.component.html',
  styleUrls: ['./card-list.component.css']
})
export class CardListComponent implements OnInit {
  @Input() cardItems: CardListItem[];

  constructor() {}

  ngOnInit() {}
}
