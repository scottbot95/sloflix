import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { UserService } from './services/user.service';
import { EqualValidator } from './directives/equal-validator.directive';
import { ResponsiveColumns } from './directives/responsive-columns.directive';
import { FlexLayoutModule } from '@angular/flex-layout';
import { UserRatingComponent } from './components/user-rating/user-rating.component';
import { MatIconModule } from '@angular/material';
import { TimesPipe } from './times.pipe';

@NgModule({
  imports: [CommonModule, FlexLayoutModule, MatIconModule],
  declarations: [
    SpinnerComponent,
    EqualValidator,
    ResponsiveColumns,
    UserRatingComponent,
    TimesPipe
  ],
  exports: [
    SpinnerComponent,
    EqualValidator,
    ResponsiveColumns,
    UserRatingComponent
  ],
  providers: [UserService]
})
export class SharedModule {}
