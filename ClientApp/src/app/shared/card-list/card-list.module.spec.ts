import { CardListModule } from './card-list.module';

describe('CardListModule', () => {
  let cardListModule: CardListModule;

  beforeEach(() => {
    cardListModule = new CardListModule();
  });

  it('should create an instance', () => {
    expect(cardListModule).toBeTruthy();
  });
});
