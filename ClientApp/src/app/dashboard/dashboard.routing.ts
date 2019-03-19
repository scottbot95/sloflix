import { ModuleWithProviders } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AuthGuard } from '../auth.guard';
import { HomeComponent } from './home/home.component';
import { WatchlistDetailsComponent } from './watchlist-details/watchlist-details.component';

export const routing: ModuleWithProviders = RouterModule.forChild([
  {
    path: 'dashboard',
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      { path: 'home', component: HomeComponent },
      { path: 'watchlist/:id', component: WatchlistDetailsComponent }
    ]
  }
]);
