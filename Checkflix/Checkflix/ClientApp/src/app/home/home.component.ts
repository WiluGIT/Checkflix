import { ProductionService } from './../../services/production.service';
import { Component, OnInit} from '@angular/core';
import { IProductionViewModel } from '../ClientViewModels/IProductionViewModel';
import {PageEvent} from '@angular/material/paginator';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  productionList: Array<IProductionViewModel>;
  length:number;
  pageSize:number = 20;
  pageEvent: PageEvent;
  activePageDataChunk:Array<IProductionViewModel> = [];

  constructor(private productionService:ProductionService){
    
  }

  ngOnInit(){
    this.productionService.getProductions()
    .subscribe(productions => {
      this.productionList = productions;
      this.length = productions.length;
      this.activePageDataChunk = this.productionList.slice(0,this.pageSize);
      console.log(this.activePageDataChunk)
    });
  }

  onPageChanged(e,htmlTarget: HTMLElement) {
    console.log(e.pageIndex)
    let firstCut = e.pageIndex * e.pageSize;
    let secondCut = firstCut + e.pageSize;
    this.activePageDataChunk = this.productionList.slice(firstCut, secondCut);
    htmlTarget.scrollIntoView({behavior:"smooth"});
  }
}
