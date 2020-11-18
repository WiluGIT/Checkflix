import { IApplicationUserProductionViewModel } from './../ClientViewModels/IApplicationUserProductionViewModel';
import { UserProducionService } from './../../services/user-producion.service';
import { IUserCollectionFilter } from './../ClientViewModels/IUserCollectionFilter';
import { ActivatedRoute, Router, RouterStateSnapshot } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import { IPostQueryFilters } from '../ClientViewModels/IPostQueryFilters';
import { IProductionViewModel } from '../ClientViewModels/IProductionViewModel';
import { MatPaginator, MatSnackBar } from '@angular/material';
import { ApplicationPaths, QueryParameterNames } from 'src/api-authorization/api-authorization.constants';

@Component({
  selector: 'app-collection-list',
  templateUrl: './collection-list.component.html',
  styleUrls: ['./collection-list.component.css']
})
export class CollectionListComponent implements OnInit {
  userQueryFilters: IUserCollectionFilter = {
    pageNumber: 1,
    pageSize: 10,
    favourites: null,
    toWatch: null,
    watched: null,
    userId: null
  };
  productionList: Array<IProductionViewModel>;
  productionsCount: number;
  activePageDataChunk: Array<IProductionViewModel>;
  collectionType: string;
  contentLoaded: boolean = false;

  @ViewChild('collectionPaginator', { static: false }) paginator: MatPaginator;
  constructor(
    private userProductionService: UserProducionService,
    route: ActivatedRoute,
    private router: Router,
    public snackBar: MatSnackBar) {
    route.params.subscribe(p => {
      this.userQueryFilters.userId = p['id'] || null;
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

        this.contentLoaded = true;
        this.productionList = response.body.data;
        this.productionsCount = headers["TotalCount"];
        this.activePageDataChunk = this.productionList;
      }, (err => {
        if (err.status == 401) {
          this.handleAuthorization(false);
        }
      }));
  }

  onPageChanged(e, htmlTarget: HTMLElement) {
    // Update current page index
    this.userQueryFilters.pageNumber = e.pageIndex + 1;
    // Call endpoint
    this.userProductionService.getUserCollection(this.userQueryFilters)
      .subscribe(response => {
        const headers = JSON.parse(response.headers.get('X-Pagination'));

        this.productionList = response.body.data;
        this.activePageDataChunk = this.productionList;
      });

    // Scroll to target
    htmlTarget.scrollIntoView({ behavior: "smooth" });
  }

  deleteFromCollection(productionId) {
    let userProduction: IApplicationUserProductionViewModel;

    if (this.collectionType === "Ulubione") {
      userProduction = {
        productionId: productionId,
        toWatch: null,
        favourites: false,
        watched: null
      };
    }
    else if (this.collectionType === "Do obejrzenia") {
      userProduction = {
        productionId: productionId,
        toWatch: false,
        favourites: null,
        watched: null
      };
    }
    else if (this.collectionType === "Obejrzane") {
      userProduction = {
        productionId: productionId,
        toWatch: null,
        favourites: null,
        watched: false
      };
    }

    this.userProductionService.addUserProduction(userProduction)
      .subscribe(response => {
        if (response['status'] == 1) {
          this.openSnackBar(response['messages'], 'Zamknij', 'blue-snackbar');

          this.userProductionService.getUserCollection(this.userQueryFilters)
            .subscribe(response => {
              const headers = JSON.parse(response.headers.get('X-Pagination'));

              this.productionsCount = headers["TotalCount"];
              this.productionList = response.body.data;
              this.activePageDataChunk = this.productionList;
            });
        } else {
          this.openSnackBar(response['messages'], 'Zamknij', 'red-snackbar');
        }
      }, (err => {
        if (err.status == 401) {
          this.handleAuthorization(false);
        }
        else {
          this.openSnackBar("Spróbuj ponownie", 'Zamknij', 'red-snackbar');
        }
      }));
  }

  markAsSeen(productionId) {
    console.log(productionId)
    const userProduction: IApplicationUserProductionViewModel = {
      productionId: productionId,
      toWatch: false,
      favourites: null,
      watched: true
    };

    this.userProductionService.addUserProduction(userProduction)
      .subscribe(response => {
        if (response['status'] == 1) {
          this.openSnackBar(response['messages'][1], 'Zamknij', 'blue-snackbar');

          this.userProductionService.getUserCollection(this.userQueryFilters)
            .subscribe(response => {
              const headers = JSON.parse(response.headers.get('X-Pagination'));

              this.productionsCount = headers["TotalCount"];
              this.productionList = response.body.data;
              this.activePageDataChunk = this.productionList;
            });
        } else {
          this.openSnackBar(response['messages'], 'Zamknij', 'red-snackbar');
        }
      }, (err => {
        if (err.status == 401) {
          this.handleAuthorization(false);
        }
        else {
          this.openSnackBar("Spróbuj ponownie", 'Zamknij', 'red-snackbar');
        }
      }));
  }

  handleAuthorization(isAuthenticated: boolean) {
    if (!isAuthenticated) {
      this.router.navigate(ApplicationPaths.LoginPathComponents)
    };
  }

  openSnackBar(message: string, action: string, className: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
      panelClass: [className]
    });
  }
}
