import { WatchlistDetailsModule } from './watchlist-details.module';

describe('WatchlistDetailsModule', () => {
  let watchlistDetailsModule: WatchlistDetailsModule;

  beforeEach(() => {
    watchlistDetailsModule = new WatchlistDetailsModule();
  });

  it('should create an instance', () => {
    expect(watchlistDetailsModule).toBeTruthy();
  });
});
