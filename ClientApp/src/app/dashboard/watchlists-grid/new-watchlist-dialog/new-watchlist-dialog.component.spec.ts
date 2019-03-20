import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewWatchlistDialogComponent } from './new-watchlist-dialog.component';

describe('NewWatchlistDialogComponent', () => {
  let component: NewWatchlistDialogComponent;
  let fixture: ComponentFixture<NewWatchlistDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewWatchlistDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewWatchlistDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
