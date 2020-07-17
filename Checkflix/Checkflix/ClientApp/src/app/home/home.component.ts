import { IPostQueryFilters } from './../ClientViewModels/IPostQueryFilters';
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
  productionsCount:number;
  postQueryFilters: IPostQueryFilters = {
    pageNumber: 1,
    pageSize: 25
  };
  activePageDataChunk:Array<IProductionViewModel> = [];

  constructor(private productionService:ProductionService){ 
  }

  ngOnInit(){
    this.productionService.getProductions(this.postQueryFilters)
    .subscribe(response => {
      const headers = JSON.parse(response.headers.get('X-Pagination'));

      this.productionList = response.body;
      this.productionsCount = headers["TotalCount"];
      this.activePageDataChunk = this.productionList;
    });
  }

  onPageChanged(e,htmlTarget: HTMLElement) {
    // Update current page index
    this.postQueryFilters.pageNumber = e.pageIndex + 1;

    // Call endpoint
    this.productionService.getProductions(this.postQueryFilters)
    .subscribe(response => {
      this.productionList = response.body;
      this.activePageDataChunk = this.productionList;
    });

    // Scroll to target
    htmlTarget.scrollIntoView({behavior:"smooth"});
  }
}
