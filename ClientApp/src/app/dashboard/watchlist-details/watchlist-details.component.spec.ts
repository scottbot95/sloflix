import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WatchlistDetailsComponent } from './watchlist-details.component';

describe('WatchlistDetailsComponent', () => {
  let component: WatchlistDetailsComponent;
  let fixture: ComponentFixture<WatchlistDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WatchlistDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WatchlistDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
