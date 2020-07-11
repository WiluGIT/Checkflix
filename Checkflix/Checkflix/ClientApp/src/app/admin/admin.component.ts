import { IProductionViewModel } from './../ClientViewModels/IProductionViewModel';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthorizeService } from '../../api-authorization/authorize.service';
import { map, filter, delay } from 'rxjs/operators';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, of } from 'rxjs';
import { ProductionService } from '../../services/production.service';
import { ICategoryViewModel } from '../ClientViewModels/ICategoryViewModel';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MatSnackBar } from '@angular/material';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { ThemePalette } from '@angular/material/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  displayedColumns: string[] = ['Id', 'Tytuł', 'Data premiery', 'Akcje'];
  dataSource: MatTableDataSource<IProductionViewModel>;
  isShowed: boolean = false;
  moreThan5result: boolean = false;
  productionList: Array<IProductionViewModel>;
  categoriesList: Array<ICategoryViewModel> = [];
  deletedProduction: IProductionViewModel;
  categoryList: ICategoryViewModel[];

  productionFromApi: IProductionViewModel = {
    productionId: 0,
    poster: "brak",
    title: "brak",
    subtitle: "brak",
    synopsis: "brak",
    type: null,
    releaseDate: null,
    imbdId: " ",
    imbdRating: null,
    vods: [],
    categories: []

  };
  productionListFromApi: Array<IProductionViewModel>;


  // XD
  color: ThemePalette = 'primary';
  mode: ProgressSpinnerMode = 'determinate';
  value = 0;


  @ViewChild(MatSort, { static: true }) sort: MatSort;
  constructor(private authService: AuthorizeService,
    private productionService: ProductionService,
    private router: Router,
    private http: HttpClient,
    public snackBar: MatSnackBar) {
    this.dataSource = new MatTableDataSource(this.productionList);
  }

  ngOnInit() {
    this.productionService
      .getProductions()
      .subscribe(productions => this.productionList = productions);

    this.productionService
      .getCategories()
      .subscribe(categories => this.categoryList = categories);

    this.dataSource.sort = this.sort;

  }

  applyFilter(event: Event) {
    //set datasource and flags
    this.dataSource = new MatTableDataSource(this.productionList);
    this.isShowed = true;
    this.moreThan5result = false;

    //filter
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.filteredData.length > 5)
      this.moreThan5result = true



    //slice 5 first results
    const data = this.dataSource.filteredData.slice(0, 5);
    this.dataSource.data = data;

  }

  deleteProduction(productionId) {
    this.productionService
      .deleteProduction(productionId)
      .subscribe(res => {
        if (res['status'] == 1) {
          this.openSnackBar(res['messages'], 'Zamknij', 'red-snackbar');
        } else {
          this.openSnackBar(res['messages'], 'Zamknij', 'green-snackbar');
        }
      },
        err => {
          if (err.status == 404) {
            //implement component with not found 404 error
            this.router.navigate(['/admin']);
          }
        });

    var filteredList: IProductionViewModel[] = this.productionList.filter(production => production.productionId !== productionId);
    this.productionList = filteredList;
    this.dataSource = new MatTableDataSource(this.dataSource.filteredData);
    const data = this.dataSource.filteredData.filter(production => production.productionId !== productionId).slice(0, 5);
    this.dataSource.data = data;

  }
  gowno() {
    console.log(this.productionListFromApi)
    // this.value = 0
    // let dataCount = 1000;
    // let counter = 0;
    // for (let i = 0; i < dataCount; i++) {
    //   counter += 1;
    //   this.value = counter / dataCount * 100; 
    // }

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

    let productionsProcessedCounter = 0;
    if (productionCount) {
      // calculate real count
      const realCount = parseInt(productionCount['COUNT'])
      const pageCount = Math.ceil(realCount / 100);
      // TMP var PAGECOUNT -> 3
      for (let i = 0; i < 3; i++) {
        const currentPage = i + 1;
        // TODO currentPage in URL IS NOT INCREMENTING
        const currentPageUrl = `https://unogs-unogs-v1.p.rapidapi.com/aaapi.cgi?q=get%3Anew99999%3APL&p=${currentPage}&t=ns&st=adv`;

        // unongs endpoint
        let currentPageData = await this.http.get(currentPageUrl, {
          headers: {
            "x-rapidapi-host": "unogs-unogs-v1.p.rapidapi.com",
            "x-rapidapi-key": "8a5735bcd6msh35b94dd1467c587p1baf48jsn33eb07d88120"
          }
        }).toPromise();

        for (let j = 0; j < currentPageData["ITEMS"].length; j++) {
          if (currentPageData["ITEMS"][j].imdbid !== "notfound" && currentPageData["ITEMS"][j].imdbid) {
            const apiData = currentPageData["ITEMS"][j];

            // themoviedb endpoint
            const movieDburl = `https://api.themoviedb.org/3/find/${apiData.imdbid}?api_key=61a4454e6812a635ebe4b24f2af2c479&language=pl_PL&external_source=imdb_id`;
            let movieDbData = await this.http.get(movieDburl).toPromise();

            // imbd endpoint
            const imbdUrl = "https://imdb-internet-movie-database-unofficial.p.rapidapi.com/film/" + apiData.imdbid;
            let imbdData = await this.http.get(imbdUrl, {
              headers: {
                'x-rapidapi-host': 'imdb-internet-movie-database-unofficial.p.rapidapi.com',
                'x-rapidapi-key': '8a5735bcd6msh35b94dd1467c587p1baf48jsn33eb07d88120'
              }
            }).toPromise();

            // update counter for progress bar
            productionsProcessedCounter += 1;
            this.value = (productionsProcessedCounter / (realCount * 2)) * 100;
            

            // gather data from api
            this.productionFromApi.poster = apiData.image;
            this.productionFromApi.title = apiData.title;

            if (movieDbData["movie_results"].length > 0) {
              const movieArray = movieDbData["movie_results"][0];

              const categories = movieArray.genre_ids.map(el =>
                this.categoryList.find(x => x.genreApiId === el)
              );
              this.productionFromApi.imbdId = apiData.imdbid;
              this.productionFromApi.categories = categories;
              this.productionFromApi.subtitle = movieArray.title;
              if (movieArray.overview)
                this.productionFromApi.synopsis = movieArray.overview;
              this.productionFromApi.type = 0;
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
              this.productionFromApi.subtitle = seriesArray.name;
              if (seriesArray.overview)
                this.productionFromApi.synopsis = seriesArray.overview;
              this.productionFromApi.type = 1;
              this.productionFromApi.releaseDate = new Date(seriesArray.first_air_date);
              this.productionFromApi.vods = [{
                vodId: 1,
                platformName: "Netflix"
              }];
            }

            if (imbdData) {
              this.productionFromApi.imbdRating = parseFloat(imbdData["rating"]);
              this.productionFromApi.poster = imbdData["poster"];
            }

            if (this.productionFromApi.imbdId && this.productionFromApi.imbdRating && this.productionFromApi.releaseDate && this.productionFromApi.type)
              this.productionListFromApi.push(this.productionFromApi)

            this.productionFromApi = {
              productionId: 0,
              poster: "brak",
              title: "brak",
              subtitle: "brak",
              synopsis: "brak",
              type: null,
              releaseDate: null,
              imbdId: " ",
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


  updateCategories() {
    const categoriesUrl = "https://api.themoviedb.org/3/genre/movie/list?api_key=61a4454e6812a635ebe4b24f2af2c479&language=pl-PL";
    fetch(categoriesUrl)
      .then(response => {
        return response.json()
      })
      .then((data) => {
        this.categoriesList = []
        let categoryElement: ICategoryViewModel;
        data.genres.map(el => {
          categoryElement = {
            categoryId: 0,
            categoryName: el.name,
            genreApiId: el.id
          };
          this.categoriesList.push(categoryElement)
        })
        this.productionService
          .updateCategories(this.categoriesList)
          .subscribe(res => {
            console.log("Added", res)
          },
            (err) => { console.log("Category list is up to date") }
          );

      })
      .catch(err => {
        console.log(err);
      });
  }

  openSnackBar(message: string, action: string, className: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
      panelClass: [className]
    });
  }


}
