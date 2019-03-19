import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WatchlistsGridComponent } from './watchlists-grid.component';

describe('WatchlistsGridComponent', () => {
  let component: WatchlistsGridComponent;
  let fixture: ComponentFixture<WatchlistsGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WatchlistsGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WatchlistsGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
