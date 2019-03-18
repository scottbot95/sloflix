import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatCardModule,
  MatGridListModule,
  MatIconModule
} from '@angular/material';
import { MaterialModule } from '../app.material.module';
import { CardListComponent } from './card-list.component';
import { SharedModule } from '../shared.module';
import { CardModule } from '../components/card/card.module';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    MatCardModule,
    MatGridListModule,
    MatIconModule,
    SharedModule,
    CardModule
  ],
  declarations: [CardListComponent],
  exports: [CardListComponent]
})
export class CardListModule {}
