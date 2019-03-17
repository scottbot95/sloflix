import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { UserService } from './services/user.service';

@NgModule({
  imports: [CommonModule],
  declarations: [SpinnerComponent],
  exports: [SpinnerComponent],
  providers: [UserService]
})
export class SharedModule {}
