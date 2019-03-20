import { NgModule } from '@angular/core';
import {
  MatButtonModule,
  MatFormFieldModule,
  MatDialogModule,
  MatIconModule,
  MatTooltipModule
} from '@angular/material';

@NgModule({
  imports: [
    MatButtonModule,
    MatFormFieldModule,
    MatDialogModule,
    MatIconModule,
    MatTooltipModule
  ],
  exports: [
    MatButtonModule,
    MatFormFieldModule,
    MatDialogModule,
    MatIconModule,
    MatTooltipModule
  ]
})
export class MaterialModule {}
