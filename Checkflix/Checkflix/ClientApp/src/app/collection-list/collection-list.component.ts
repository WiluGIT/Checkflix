import { ActivatedRoute, Router } from '@angular/router';
import { ProductionService } from './../../services/production.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { IPostQueryFilters } from '../ClientViewModels/IPostQueryFilters';
import { IProductionViewModel } from '../ClientViewModels/IProductionViewModel';
import { MatPaginator } from '@angular/material';

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
  collectionType:string;

  @ViewChild('collectionPaginator', { static: false }) paginator: MatPaginator;
  constructor(
    private productionService: ProductionService,
    route: ActivatedRoute,
    private router: Router) {
    route.params.subscribe(p => {
      let collectionName = p['collectionName'];
      if (collectionName === "favourites") {
        this.collectionType = "Ulubione";
      }
      else if (collectionName === "to-watch") {
        this.collectionType = "Do obejrzenia";
      }
      else if (collectionName === "watched") {
        this.collectionType = "Obejrzane";
      }
      else {
        this.router.navigate(['/']);
      }
    });
  }

  ngOnInit() {
    this.productionService.getProductions(this.postQueryFilters)
      .subscribe(response => {
        const headers = JSON.parse(response.headers.get('X-Pagination'));

        this.productionList = response.body;
        this.productionsCount = headers["TotalCount"];
        this.activePageDataChunk = this.productionList;
      });
  }

  onPageChanged(e, htmlTarget: HTMLElement) {
    // Update current page index
    this.postQueryFilters.pageNumber = e.pageIndex + 1;
    // Call endpoint
    this.productionService.getProductions(this.postQueryFilters)
      .subscribe(response => {
        this.productionList = response.body;
        this.activePageDataChunk = this.productionList;
      });

    // Scroll to target
    htmlTarget.scrollIntoView({ behavior: "smooth" });
  }

}
