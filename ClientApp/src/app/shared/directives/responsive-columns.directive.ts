import { Directive, OnInit } from '@angular/core';
import { MatGridList } from '@angular/material';
import { ObservableMedia, MediaChange } from '@angular/flex-layout';
import { Input } from '@angular/core';

export interface ResponsiveColumnsMap {
  xs?: number;
  sm?: number;
  md?: number;
  lg?: number;
  xl?: number;
}

@Directive({
  selector: '[responsiveCols]'
})
export class ResponsiveColumns implements OnInit {
  private colsBySize: ResponsiveColumnsMap = {
    xs: 1,
    sm: 1,
    md: 1,
    lg: 1,
    xl: 1
  };

  public get cols(): ResponsiveColumnsMap {
    return this.colsBySize;
  }

  @Input('responsiveCols')
  public set cols(value: ResponsiveColumnsMap) {
    if (value) {
      this.colsBySize = value;
      this.fixColSizes();
      this.initializeColCount();
    }
  }

  constructor(private grid: MatGridList, private media: ObservableMedia) {
    this.initializeColCount();
  }

  ngOnInit(): void {
    this.fixColSizes();
    this.initializeColCount();
    this.media.asObservable().subscribe((change: MediaChange) => {
      this.grid.cols = this.colsBySize[change.mqAlias];
      console.log(change, this.grid.cols);
    });
  }

  private initializeColCount() {
    Object.keys(this.colsBySize).some(mqAlias => {
      const isActive = this.media.isActive(mqAlias);
      if (isActive) this.grid.cols = this.colsBySize[mqAlias];
      return isActive;
    });
  }

  private fixColSizes() {
    let colCount = this.colsBySize.xs;
    this.colsBySize.sm = this.colsBySize.sm || colCount;
    colCount = this.colsBySize.sm;
    this.colsBySize.md = this.colsBySize.md || colCount;
    colCount = this.colsBySize.md;
    this.colsBySize.lg = this.colsBySize.lg || colCount;
    colCount = this.colsBySize.lg;
    this.colsBySize.xl = this.colsBySize.xl || colCount;
    colCount = this.colsBySize.xl;
  }
}
