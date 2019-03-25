import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-user-rating',
  templateUrl: './user-rating.component.html',
  styleUrls: ['./user-rating.component.css']
})
export class UserRatingComponent implements OnInit {
  @Input() rating: number = 0;

  constructor() {}

  ngOnInit() {}
}
