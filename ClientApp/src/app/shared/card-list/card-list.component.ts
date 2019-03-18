import { Component, OnInit, Input } from '@angular/core';
import { CardDetails } from '../components/card/card.component';

@Component({
  selector: 'app-card-list',
  templateUrl: './card-list.component.html',
  styleUrls: ['./card-list.component.css']
})
export class CardListComponent implements OnInit {
  @Input() cardItems: CardDetails[];

  constructor() {}

  ngOnInit() {}
}
