import { Component, OnInit, Input, OnChanges, OnDestroy } from '@angular/core';
import { BehaviorSubject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-user-rating',
  templateUrl: './user-rating.component.html',
  styleUrls: ['./user-rating.component.css']
})
export class UserRatingComponent implements OnChanges, OnInit, OnDestroy {
  @Input() rating: number = 0;
  @Input() setRating: (number) => void;

  private displayRating: number = 0;
  private ratingHover = new BehaviorSubject<number>(0);
  private subscription: Subscription;

  constructor() {}

  ngOnChanges() {
    this.displayRating = this.rating;
  }

  ngOnInit() {
    this.subscription = this.ratingHover
      .pipe(
        debounceTime(50),
        distinctUntilChanged()
      )
      .subscribe(r => this.setDisplayRating(r));
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  handleHover(rating) {
    this.ratingHover.next(rating);
  }

  handleClick(rating) {
    if (this.rating === rating) {
      rating = 0;
    }
    this.rating = rating;
    if (typeof this.setRating === 'function') {
      this.setRating(rating);
    }
  }

  setDisplayRating(rating) {
    this.displayRating = rating;
  }
}
