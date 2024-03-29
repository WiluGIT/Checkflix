import { AuthorizeService } from './../../api-authorization/authorize.service';
import { UserProducionService } from './../../services/user-producion.service';
import { CategoryService } from './../../services/category.service';
import { MultiSliderComponent } from './../multi-slider/multi-slider.component';
import { FormGroup, FormBuilder, NumberValueAccessor } from '@angular/forms';
import { IPostQueryFilters } from './../ClientViewModels/IPostQueryFilters';
import { ProductionService } from './../../services/production.service';
import { Component, OnInit, ViewChild, AfterViewInit, Inject } from '@angular/core';
import { IProductionViewModel } from '../ClientViewModels/IProductionViewModel';
import { MatPaginator, MatSnackBar } from '@angular/material';
import { DOCUMENT } from '@angular/common';
import { ICategoryViewModel } from '../ClientViewModels/ICategoryViewModel';
import { IApplicationUserProductionViewModel } from '../ClientViewModels/IApplicationUserProductionViewModel';
import { ApplicationPaths } from 'src/api-authorization/api-authorization.constants';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {
  productionList: Array<IProductionViewModel>;
  userProductionsList: Array<IApplicationUserProductionViewModel>;
  productionsCount: number;
  postQueryFilters: IPostQueryFilters = {
    pageNumber: 1,
    pageSize: 30,
    searchQuery: null,
    isNetflix: null,
    isHbo: null,
    yearFrom: null,
    yearTo: null,
    ratingFrom: null,
    ratingTo: null,
    categories: null,
    type: null
  };
  producionTypes = {
    Movie: {
      path: "/movies",
      value: 0
    },
    Series: {
      path: "/series",
      value: 1
    }
  }
  productionTypeText:string = "Filmy i seriale";
  categoryList: ICategoryViewModel[];
  activePageDataChunk: Array<IProductionViewModel>;
  isAuthenticated: boolean;
  // Filter form
  productionsFilterForm: FormGroup;

  //Child component slider
  minValueYear: number;
  maxValueYear: number;
  minImbdRating: number;
  maxImbdRating: number;
  //Vods icons
  netflixClicked: boolean = true;
  hboClicked: boolean = true;
  contentloaded: boolean = false;

  @ViewChild('homePaginator', { static: false }) paginator: MatPaginator;
  @ViewChild('ratingSlider', { static: false }) ratingSlider: MultiSliderComponent;
  @ViewChild('yearSlider', { static: false }) yearSlider: MultiSliderComponent;
  constructor(
    private productionService: ProductionService,
    private categoryService: CategoryService,
    private userProductionService: UserProducionService,
    private authorizeService: AuthorizeService,
    public snackBar: MatSnackBar,
    private fb: FormBuilder,
    private router: Router,
    @Inject(DOCUMENT) document) {
      if (this.router.url === this.producionTypes.Movie.path) {
        this.postQueryFilters.type = this.producionTypes.Movie.value;
        this.productionTypeText = "Filmy";
      }
      else if (this.router.url === this.producionTypes.Series.path) {
        this.postQueryFilters.type = this.producionTypes.Series.value;
        this.productionTypeText = "Seriale";
      }
  }

  ngOnInit() {
    //Endpoint calls
    this.productionService.getProductions(this.postQueryFilters)
      .subscribe(response => {
        const headers = JSON.parse(response.headers.get('X-Pagination'));

        this.contentloaded = true;
        this.productionList = response.body;
        this.productionsCount = headers["TotalCount"];
        this.activePageDataChunk = this.productionList;
      });

    // Categories
    this.categoryService
      .getCategories()
      .subscribe(categories => this.categoryList = categories)

    this.authorizeService.isAuthenticated()
      .subscribe(authenticated => {
        this.isAuthenticated = authenticated;
        if (authenticated) {
          this.userProductionService.getUserProductions()
            .subscribe(responseData => {
              this.userProductionsList = responseData["data"];
            });
        }
      });

    this.productionsFilterForm = this.fb.group({
      searchQuery: null,
      isNetflix: null,
      isHbo: null,
      yearFrom: null,
      yearTo: null,
      ratingFrom: null,
      ratingTo: null,
      categories: null
    });

    //Set variables
    this.maxValueYear = new Date().getFullYear();
    this.minValueYear = 1900;
    this.minImbdRating = 0;
    this.maxImbdRating = 10;
  }

  getYearSliderValues(valueEmitted) {
    this.productionsFilterForm.controls["yearFrom"].setValue(parseInt(valueEmitted['left']));
    this.productionsFilterForm.controls["yearTo"].setValue(parseInt(valueEmitted['right']));
  }

  getImbdSliderValues(valueEmitted) {
    this.productionsFilterForm.controls["ratingFrom"].setValue(parseInt(valueEmitted['left']));
    this.productionsFilterForm.controls["ratingTo"].setValue(parseInt(valueEmitted['right']));
  }

  checkBtn(e) {
    const target = e.currentTarget;
    if (target.id === "hbo-btn") {
      if (this.hboClicked) {
        target.classList.add("vod-gray");
        target.classList.remove("hbo-btn");
        this.hboClicked = false;
        this.productionsFilterForm.controls["isHbo"].setValue(false);
      } else {
        target.classList.remove("vod-gray");
        target.classList.add("hbo-btn");
        this.hboClicked = true;
        this.productionsFilterForm.controls["isHbo"].setValue(null);
        this.postQueryFilters.isHbo = null;
      }
    } else if (target.id === "netflix-btn") {
      if (this.netflixClicked) {
        target.classList.add("vod-gray");
        target.classList.remove("netflix-btn");
        this.netflixClicked = false;
        this.productionsFilterForm.controls["isNetflix"].setValue(false);
      } else {
        target.classList.remove("vod-gray");
        target.classList.add("netflix-btn");
        this.netflixClicked = true;
        this.productionsFilterForm.controls["isNetflix"].setValue(null);
        this.postQueryFilters.isNetflix = null;
      }
    }
    // At least one element must be clicked
    if (!this.hboClicked && !this.netflixClicked) {
      this.resetVodButtons();
      this.productionsFilterForm.controls["isNetflix"].setValue(null);
      this.productionsFilterForm.controls["isHbo"].setValue(null);
    }
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

  applyFilters() {
    // Apply additional filters to query params
    if (this.productionsFilterForm.controls["searchQuery"].value) {
      this.postQueryFilters["searchQuery"] = this.productionsFilterForm.controls["searchQuery"].value;
    }
    if (this.productionsFilterForm.controls["yearFrom"].value > 1900 || this.productionsFilterForm.controls["yearTo"].value < new Date().getFullYear()) {
      this.postQueryFilters["yearFrom"] = this.productionsFilterForm.controls["yearFrom"].value;
      this.postQueryFilters["yearTo"] = this.productionsFilterForm.controls["yearTo"].value;
    }
    if (this.productionsFilterForm.controls["ratingFrom"].value > 0 || this.productionsFilterForm.controls["ratingTo"].value < 10) {
      this.postQueryFilters["ratingFrom"] = this.productionsFilterForm.controls["ratingFrom"].value;
      this.postQueryFilters["ratingTo"] = this.productionsFilterForm.controls["ratingTo"].value;
    }
    if (this.productionsFilterForm.controls["isHbo"].value == false || this.productionsFilterForm.controls["isNetflix"].value == false) {
      this.postQueryFilters["isHbo"] = this.productionsFilterForm.controls["isHbo"].value;
      this.postQueryFilters["isNetflix"] = this.productionsFilterForm.controls["isNetflix"].value;
    }
    if (this.productionsFilterForm.controls["categories"].value) {
      const categories = this.productionsFilterForm.controls["categories"].value.map(el =>
        this.categoryList.find(x => x.categoryName === el).categoryId
      );
      this.postQueryFilters["categories"] = categories;
    }
    // Change params to default
    this.postQueryFilters.pageNumber = 1;
    this.postQueryFilters.pageSize = 30;
    //Change page to first
    this.paginator.pageIndex = 0;

    this.productionService.getProductions(this.postQueryFilters)
      .subscribe(response => {
        const headers = JSON.parse(response.headers.get('X-Pagination'));

        this.productionList = response.body;
        this.productionsCount = headers["TotalCount"];
        this.activePageDataChunk = this.productionList;
        this.postQueryFilters.searchQuery = null;
      });
  }

  clearFilters() {
    this.productionsFilterForm.reset();
    this.resetVodButtons();
    this.ratingSlider.resetValues();
    this.yearSlider.resetValues();
    this.postQueryFilters = {
      pageNumber: 1,
      pageSize: 30,
      searchQuery: null,
      isNetflix: null,
      isHbo: null,
      yearFrom: null,
      yearTo: null,
      ratingFrom: null,
      ratingTo: null,
      categories: null,
      type: this.postQueryFilters.type
    };
  }

  resetVodButtons() {
    this.hboClicked = true;
    this.netflixClicked = true;
    let hboBtn = document.getElementById('hbo-btn');
    let netflixBtn = document.getElementById('netflix-btn');
    hboBtn.classList.remove("vod-gray");
    netflixBtn.classList.remove("vod-gray");
    hboBtn.classList.add("hbo-btn");
    netflixBtn.classList.add("netflix-btn");
  }

  addToWatch(productionId, e) {
    const toWatchButton = e.currentTarget;
    let userProduction: IApplicationUserProductionViewModel;
    if (toWatchButton.classList.contains('to-watch-y')) {
      userProduction = {
        productionId: productionId,
        toWatch: false,
        favourites: null,
        watched: null
      };
    }
    else {
      userProduction = {
        productionId: productionId,
        toWatch: true,
        favourites: null,
        watched: null
      };
    }
    this.userProductionService.addUserProduction(userProduction)
      .subscribe(response => {
        if (response['status'] == 1) {
          this.openSnackBar(response['messages'], 'Zamknij', 'blue-snackbar');
          toWatchButton.classList.remove("to-watch-y");
        } else {
          this.openSnackBar(response['messages'], 'Zamknij', 'green-snackbar');
          toWatchButton.classList.add("to-watch-y");
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

  addToFavourites(productionId, e) {
    const toWatchButton = e.currentTarget;
    let userProduction: IApplicationUserProductionViewModel;
    if (toWatchButton.classList.contains('to-watch-y')) {
      userProduction = {
        productionId: productionId,
        toWatch: null,
        favourites: false,
        watched: null
      };
    }
    else {
      userProduction = {
        productionId: productionId,
        toWatch: null,
        favourites: true,
        watched: null
      };
    }
    this.userProductionService.addUserProduction(userProduction)
      .subscribe(response => {
        if (response['status'] == 1) {
          this.openSnackBar(response['messages'], 'Zamknij', 'blue-snackbar');
          toWatchButton.classList.remove("to-watch-y");
        } else {
          this.openSnackBar(response['messages'], 'Zamknij', 'green-snackbar');
          toWatchButton.classList.add("to-watch-y");
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

  checkIfExistsToWatch(productionId) {
    if (this.userProductionsList) {
      return this.userProductionsList.some(el => el.productionId == productionId && el.toWatch == true)
    }
  }

  checkIfExistsFavourites(productionId) {
    if (this.userProductionsList) {
      return this.userProductionsList.some(el => el.productionId == productionId && el.favourites == true)
    }
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
