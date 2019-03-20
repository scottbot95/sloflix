import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { UserService } from './services/user.service';
import { EqualValidator } from './directives/equal-validator.directive';
import { ResponsiveColumns } from './directives/responsive-columns.directive';
import { FlexLayoutModule } from '@angular/flex-layout';

@NgModule({
  imports: [CommonModule, FlexLayoutModule],
  declarations: [SpinnerComponent, EqualValidator, ResponsiveColumns],
  exports: [SpinnerComponent, EqualValidator, ResponsiveColumns],
  providers: [UserService]
})
export class SharedModule {}
