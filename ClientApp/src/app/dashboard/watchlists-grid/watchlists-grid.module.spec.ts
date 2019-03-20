import { WatchlistsGridModule } from './watchlists-grid.module';

describe('WatchlistsGridModule', () => {
  let watchlistsGridModule: WatchlistsGridModule;

  beforeEach(() => {
    watchlistsGridModule = new WatchlistsGridModule();
  });

  it('should create an instance', () => {
    expect(watchlistsGridModule).toBeTruthy();
  });
});
