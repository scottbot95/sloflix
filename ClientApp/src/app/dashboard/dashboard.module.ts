import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { FormsModule } from '@angular/forms';
import { routing } from './dashboard.routing';
import { SharedModule } from '../shared/shared.module';
import { MaterialModule } from '../shared/app.material.module';
import { AuthGuard } from '../auth.guard';
import { WatchlistService } from '../shared/services/watchlist.service';
import { CardListModule } from '../shared/card-list/card-list.module';
import { UserService } from '../shared/services/user.service';
import { ApiService } from '../shared/services/api.service';
import { WatchlistsGridModule } from './watchlists-grid/watchlists-grid.module';
import { WatchlistDetailsModule } from './watchlist-details/watchlist-details.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    routing,
    SharedModule,
    MaterialModule,
    CardListModule,
    WatchlistsGridModule,
    WatchlistDetailsModule
  ],
  declarations: [HomeComponent],
  providers: [AuthGuard, UserService, WatchlistService, ApiService]
})
export class DashboardModule {}
