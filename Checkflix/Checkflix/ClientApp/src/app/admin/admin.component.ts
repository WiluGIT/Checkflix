import { CategoryService } from './../../services/category.service';
import { IVodCountViewModel } from 'src/app/ClientViewModels/IVodCountViewModel';
import { VodService } from './../../services/vod.service';
import { IProductionViewModel } from './../ClientViewModels/IProductionViewModel';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthorizeService } from '../../api-authorization/authorize.service';
import { map, filter, delay } from 'rxjs/operators';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, of } from 'rxjs';
import { ProductionService } from '../../services/production.service';
import { ICategoryViewModel } from '../ClientViewModels/ICategoryViewModel';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MatSnackBar } from '@angular/material';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { ThemePalette } from '@angular/material/core';
import { IPostQueryFilters } from '../ClientViewModels/IPostQueryFilters';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  // Data table
  displayedColumns: string[] = ['Id', 'Tytuł', 'Data premiery', 'Akcje'];
  dataSource: MatTableDataSource<IProductionViewModel>;
  productionList: Array<IProductionViewModel>;
  deletedProduction: IProductionViewModel;

  // Api call
  categoryList: ICategoryViewModel[];
  productionFromApi: IProductionViewModel = {
    poster: "brak",
    title: "brak",
    subtitle: "brak",
    synopsis: "brak",
    type: null,
    releaseDate: null,
    imbdId: "",
    imbdRating: null,
    vods: [],
    categories: []

  };
  productionListFromApi: Array<IProductionViewModel>;
  vodsCount: IVodCountViewModel = {
    netflixCount: 0,
    hboCount: 0
  };

  // Filter
  postQueryFilters: IPostQueryFilters = {
    pageNumber: 1,
    pageSize: 10,
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
  productionsFilterForm: FormGroup;
  productionsCount: number;

  // Progress bar
  color: ThemePalette = 'primary';
  mode: ProgressSpinnerMode = 'determinate';
  value = 0;
  productionsProcessedCounter: number = 0;
  realCount: number = 0;

  constructor(private authService: AuthorizeService,
    private productionService: ProductionService,
    private vodService: VodService,
    private categoryService: CategoryService,
    private router: Router,
    private http: HttpClient,
    public snackBar: MatSnackBar,
    private fb: FormBuilder) { }

  ngOnInit() {
    // Productions
    this.productionService.getProductions(this.postQueryFilters)
      .subscribe(response => {
        const headers = JSON.parse(response.headers.get('X-Pagination'));
        this.productionList = response.body;
        this.productionsCount = headers["TotalCount"];
        //datasource for tab
        this.dataSource = new MatTableDataSource(this.productionList);
      });

    // Categories
    this.categoryService
      .getCategories()
      .subscribe(categories => this.categoryList = categories);

    // Vods 
    this.vodService.getVodCount()
      .subscribe(response => {
        this.vodsCount.netflixCount = response.netflixCount;
        this.vodsCount.hboCount = response.hboCount;
      });

    this.productionsFilterForm = this.fb.group({
      searchQuery: ''
    });
  }

  onPageChanged(e) {
    // Update current page index
    this.postQueryFilters.pageNumber = e.pageIndex + 1;

    // Call endpoint
    this.productionService.getProductions(this.postQueryFilters)
      .subscribe(response => {
        this.productionList = response.body;
        this.dataSource = new MatTableDataSource(this.productionList);
      });
  }

  applyFilters() {
    // Apply additional filters to query params
    if (this.productionsFilterForm.controls["searchQuery"].value) {
      this.postQueryFilters["searchQuery"] = this.productionsFilterForm.controls["searchQuery"].value;
    }

    // Change params to default
    this.postQueryFilters.pageNumber = 1;
    this.postQueryFilters.pageSize = 10;


    this.productionService.getProductions(this.postQueryFilters)
      .subscribe(response => {
        const headers = JSON.parse(response.headers.get('X-Pagination'));

        this.productionList = response.body;
        this.productionsCount = headers["TotalCount"];
        this.dataSource = new MatTableDataSource(this.productionList);
      });

    // Reset fields
    this.postQueryFilters.searchQuery = null;
  }

  deleteProduction(productionId) {
    this.productionService
      .deleteProduction(productionId)
      .subscribe(res => {
        if (res['status'] == 1) {
          this.openSnackBar(res['messages'], 'Zamknij', 'red-snackbar');
        } else {
          var filteredList: IProductionViewModel[] = this.productionList.filter(production => production.productionId !== productionId);
          this.productionList = filteredList;
          this.productionsCount -= 1;
          this.dataSource = new MatTableDataSource(this.productionList);

          // Notification
          this.openSnackBar(res['messages'], 'Zamknij', 'green-snackbar');

        }
      },
        err => {
          if (err.status == 404) {
            //implement component with not found 404 error
            this.router.navigate(['/admin']);
          }
        });
  }

  async fetchNetflix() {
    this.productionListFromApi = [];
    // unongs endpoint
    const unongsUrl = "https://unogs-unogs-v1.p.rapidapi.com/aaapi.cgi?q=get%3Anew99999%3APL&p=1&t=ns&st=adv";
    let productionCount = await this.http.get(unongsUrl, {
      headers: {
        "x-rapidapi-host": "unogs-unogs-v1.p.rapidapi.com",
        "x-rapidapi-key": "8a5735bcd6msh35b94dd1467c587p1baf48jsn33eb07d88120"
      }
    }).toPromise();

    if (productionCount) {
      this.realCount = parseInt(productionCount['COUNT']);
      // this.realCount = 1000;
      this.productionsProcessedCounter = 0;
      const pageCount = Math.ceil(this.realCount / 100);
      // const pageCount = 10; 
      for (let i = 0; i < pageCount; i++) {
        const currentPage = i + 1;

        const currentPageUrl = `https://unogs-unogs-v1.p.rapidapi.com/aaapi.cgi?q=get%3Anew99999%3APL&p=${currentPage}&t=ns&st=adv`;

        // unongs endpoint
        let currentPageData = await this.http.get(currentPageUrl, {
          headers: {
            "x-rapidapi-host": "unogs-unogs-v1.p.rapidapi.com",
            "x-rapidapi-key": "8a5735bcd6msh35b94dd1467c587p1baf48jsn33eb07d88120"
          }
        }).toPromise();

        for (let j = 0; j < currentPageData["ITEMS"].length; j++) {
          // update counter for progress bar
          this.productionsProcessedCounter += 1;
          console.log(this.productionsProcessedCounter)
          this.value = (this.productionsProcessedCounter / this.realCount) * 100;

          if (currentPageData["ITEMS"][j].imdbid !== "notfound" && currentPageData["ITEMS"][j].imdbid) {
            const apiData = currentPageData["ITEMS"][j];

            // themoviedb endpoint
            const movieDburl = `https://api.themoviedb.org/3/find/${apiData.imdbid}?api_key=61a4454e6812a635ebe4b24f2af2c479&language=pl-PL&external_source=imdb_id`;
            let movieDbData = await this.http.get(movieDburl).toPromise();

            // imbd endpoint
            const imbdUrl = "https://imdb-internet-movie-database-unofficial.p.rapidapi.com/film/" + apiData.imdbid;
            let imbdData = await this.http.get(imbdUrl, {
              headers: {
                'x-rapidapi-host': 'imdb-internet-movie-database-unofficial.p.rapidapi.com',
                'x-rapidapi-key': '8a5735bcd6msh35b94dd1467c587p1baf48jsn33eb07d88120'
              }
            }).toPromise();

            // gather data from api
            if (apiData.image)
              this.productionFromApi.poster = apiData.image;
            if (apiData.title)
              this.productionFromApi.title = apiData.title;

            if (movieDbData["movie_results"].length > 0) {
              const movieArray = movieDbData["movie_results"][0];

              const categories = movieArray.genre_ids.map(el =>
                this.categoryList.find(x => x.genreApiId === el)
              );
              this.productionFromApi.imbdId = apiData.imdbid;
              this.productionFromApi.categories = categories;
              if (movieArray.title)
                this.productionFromApi.subtitle = movieArray.title;
              if (movieArray.overview)
                this.productionFromApi.synopsis = movieArray.overview;
              this.productionFromApi.type = 0;
              if (movieArray.release_date)
                this.productionFromApi.releaseDate = new Date(movieArray.release_date);
              this.productionFromApi.vods = [{
                vodId: 1,
                platformName: "Netflix"
              }];

            }
            else if (movieDbData["tv_results"].length > 0) {
              const seriesArray = movieDbData["tv_results"][0];


              const categories = seriesArray.genre_ids.map(el =>
                this.categoryList.find(x => x.genreApiId === el)
              );
              this.productionFromApi.imbdId = apiData.imdbid;
              this.productionFromApi.categories = categories;
              if (seriesArray.name)
                this.productionFromApi.subtitle = seriesArray.name;
              if (seriesArray.overview)
                this.productionFromApi.synopsis = seriesArray.overview;
              this.productionFromApi.type = 1;
              if (seriesArray.first_air_date)
                this.productionFromApi.releaseDate = new Date(seriesArray.first_air_date);
              this.productionFromApi.vods = [{
                vodId: 1,
                platformName: "Netflix"
              }];
            }

            if (imbdData) {
              this.productionFromApi.imbdRating = parseFloat(imbdData["rating"]);
              if (this.productionFromApi.poster === "brak" && imbdData["poster"])
                this.productionFromApi.poster = imbdData["poster"];
            }

            if (this.productionFromApi.imbdId
              && this.productionFromApi.imbdRating
              && this.productionFromApi.releaseDate
              && this.productionFromApi.type
              && this.productionFromApi.categories.length > 0) {
              this.productionListFromApi.push(this.productionFromApi)
            }

            this.productionFromApi = {
              poster: "brak",
              title: "brak",
              subtitle: "brak",
              synopsis: "brak",
              type: null,
              releaseDate: null,
              imbdId: "",
              imbdRating: null,
              vods: [],
              categories: []

            };
          }
        }
      }
      this.productionService
        .createProductions(this.productionListFromApi)
        .subscribe(res => {
          if (res['status'] == 1) {
            this.openSnackBar(res['messages'], 'Zamknij', 'red-snackbar');
            this.value = 0;
          } else {
            this.openSnackBar(res['messages'], 'Zamknij', 'green-snackbar');
            this.value = 0;
          }
        });
    }
  }

  openSnackBar(message: string, action: string, className: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
      panelClass: [className]
    });
  }


}
