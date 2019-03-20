import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule, MatIconModule } from '@angular/material';
import { CardComponent } from './card.component';
import { MaterialModule } from '../../app.material.module';
import { RouterModule } from '@angular/router';

@NgModule({
  imports: [CommonModule, MaterialModule, MatCardModule, RouterModule],
  declarations: [CardComponent],
  exports: [CardComponent]
})
export class CardModule {}
