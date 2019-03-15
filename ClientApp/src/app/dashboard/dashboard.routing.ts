import { ModuleWithProviders } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AuthGuard } from '../auth.guard';
import { HomeComponent } from './home/home.component';

export const routing: ModuleWithProviders = RouterModule.forChild([
  {
    path: 'dashboard',
    canActivate: [AuthGuard],
    children: [
      { path: '', component: HomeComponent },
      { path: 'home', component: HomeComponent }
    ]
  }
]);
