import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { UserService } from './services/user.service';
import { EqualValidator } from './directives/equal-validator.directive';

@NgModule({
  imports: [CommonModule],
  declarations: [SpinnerComponent, EqualValidator],
  exports: [SpinnerComponent, EqualValidator],
  providers: [UserService]
})
export class SharedModule {}
