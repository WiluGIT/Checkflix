import { ProductionService } from './../../services/production.service';
import { Component, OnInit } from '@angular/core';
import { IPostQueryFilters } from '../ClientViewModels/IPostQueryFilters';
import { IProductionViewModel } from '../ClientViewModels/IProductionViewModel';

@Component({
  selector: 'app-collection-list',
  templateUrl: './collection-list.component.html',
  styleUrls: ['./collection-list.component.css']
})
export class CollectionListComponent implements OnInit {
  postQueryFilters: IPostQueryFilters = {
    pageNumber: 1,
    pageSize: 25,
    searchQuery: null,
    isNetflix: null,
    isHbo: null,
    yearFrom: null,
    yearTo: null,
    ratingFrom: null,
    ratingTo: null,
    categories: null
  };
  productionList: Array<IProductionViewModel>;
  productionsCount: number;
  activePageDataChunk: Array<IProductionViewModel>;


  constructor(private productionService:ProductionService) { }

  ngOnInit() {
    this.productionService.getProductions(this.postQueryFilters)
    .subscribe(response => {
      const headers = JSON.parse(response.headers.get('X-Pagination'));

      this.productionList = response.body;
      this.productionsCount = headers["TotalCount"];
      this.activePageDataChunk = this.productionList;
    });
  }

}
