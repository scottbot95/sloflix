import { NgModule } from '@angular/core';
import { MatButtonModule, MatFormFieldModule } from '@angular/material';

@NgModule({
  imports: [MatButtonModule, MatFormFieldModule],
  exports: [MatButtonModule, MatFormFieldModule]
})
export class MaterialModule {}
