import { UserProducionService } from './../../services/user-producion.service';
import { IUserCollectionFilter } from './../ClientViewModels/IUserCollectionFilter';
import { ActivatedRoute, Router } from '@angular/router';
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
  userQueryFilters: IUserCollectionFilter = {
    pageNumber: 1,
    pageSize: 25,
    favourites: null,
    toWatch: null,
    watched: null
  };
  productionList: Array<IProductionViewModel>;
  productionsCount: number;
  activePageDataChunk: Array<IProductionViewModel>;
  collectionType:string;

  @ViewChild('collectionPaginator', { static: false }) paginator: MatPaginator;
  constructor(
    private userProductionService: UserProducionService,
    route: ActivatedRoute,
    private router: Router) {
    route.params.subscribe(p => {
      let collectionName = p['collectionName'];
      if (collectionName === "favourites") {
        this.collectionType = "Ulubione";
        this.userQueryFilters.favourites = true;
      }
      else if (collectionName === "to-watch") {
        this.collectionType = "Do obejrzenia";
        this.userQueryFilters.toWatch = true;
      }
      else if (collectionName === "watched") {
        this.collectionType = "Obejrzane";
        this.userQueryFilters.watched = true;
      }
      else {
        this.router.navigate(['/']);
      }
    });
  }

  ngOnInit() {
    this.userProductionService.getUserCollection(this.userQueryFilters)
      .subscribe(response => {
        const headers = JSON.parse(response.headers.get('X-Pagination'));

        this.productionList = response.body.data;
        this.productionsCount = headers["TotalCount"];
        this.activePageDataChunk = this.productionList;
      });
  }

  onPageChanged(e, htmlTarget: HTMLElement) {
    // Update current page index
    this.userQueryFilters.pageNumber = e.pageIndex + 1;
    // Call endpoint
    this.userProductionService.getUserCollection(this.userQueryFilters)
      .subscribe(response => {
        this.productionList = response.body.data;
        this.activePageDataChunk = this.productionList;
      });

    // Scroll to target
    htmlTarget.scrollIntoView({ behavior: "smooth" });
  }
}
