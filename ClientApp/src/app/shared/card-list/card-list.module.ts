import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatCardModule,
  MatGridListModule,
  MatIconModule
} from '@angular/material';
import { MaterialModule } from '../app.material.module';
import { CardListComponent } from './card-list.component';
import { CardListItemComponent } from './card-list-item/card-list-item.component';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    MatCardModule,
    MatGridListModule,
    MatIconModule
  ],
  declarations: [CardListComponent, CardListItemComponent],
  exports: [CardListComponent]
})
export class CardListModule {}
