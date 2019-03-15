import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { SpinnerComponent } from './shared/components/spinner/spinner.component';

import { routing } from './app.routing';
import { AccountModule } from './account/account.module';
import { ConfigService } from './shared/services/config.service';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [AppComponent, NavMenuComponent, HomeComponent],
  imports: [
    AccountModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    routing
  ],
  providers: [ConfigService],
  bootstrap: [AppComponent]
})
export class AppModule {}
