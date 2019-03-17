import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { FormsModule } from '@angular/forms';
import { routing } from './dashboard.routing';
import { SharedModule } from '../shared/shared.module';
import { MaterialModule } from '../shared/app.material.module';
import { AuthGuard } from '../auth.guard';
import { WatchlistService } from '../shared/services/watchlist.service';
import { WatchlistsGridComponent } from './watchlists-grid/watchlists-grid.component';

@NgModule({
  imports: [CommonModule, FormsModule, routing, SharedModule, MaterialModule],
  declarations: [HomeComponent, WatchlistsGridComponent],
  providers: [AuthGuard, WatchlistService]
})
export class DashboardModule {}
