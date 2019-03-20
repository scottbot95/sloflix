import { NgModule } from '@angular/core';
import {
  MatButtonModule,
  MatFormFieldModule,
  MatDialogModule,
  MatIconModule
} from '@angular/material';

@NgModule({
  imports: [
    MatButtonModule,
    MatFormFieldModule,
    MatDialogModule,
    MatIconModule
  ],
  exports: [MatButtonModule, MatFormFieldModule, MatDialogModule, MatIconModule]
})
export class MaterialModule {}
