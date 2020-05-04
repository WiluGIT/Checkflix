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
  fetchedProductionList: Array<IProductionViewModel> = [];
  categoryList: ICategoryViewModel[];
  productionFromApi: IProductionViewModel = {
    productionId: 0,
    poster: " ",
    title: " ",
    synopsis: "brak",
    type: null,
    releaseDate: null,
    imbdId: " ",
    imbdRating: null,
    vods: [],
    categories: []

  };
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  constructor(private authService: AuthorizeService, private productionService: ProductionService, private router: Router) {
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
    console.log(this.productionList);

    this.productionService
      .deleteProduction(productionId)
      .subscribe(deleted => {
        console.log(deleted)
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
  fetchNetflix() {
    // const fetchedCount = this.getNetflixProductionCount();
    // let count = Math.ceil(4253 / 100);
    this.getImbdRating("beczka")

  }
  gowno() {
    console.log(this.fetchedProductionList);
  }

  getNetflixProductionCount() {
    fetch("https://unogs-unogs-v1.p.rapidapi.com/aaapi.cgi?q=get%3Anew99999%3APL&p=1&t=ns&st=adv", {
      "method": "GET",
      "headers": {
        "x-rapidapi-host": "unogs-unogs-v1.p.rapidapi.com",
        "x-rapidapi-key": "8a5735bcd6msh35b94dd1467c587p1baf48jsn33eb07d88120"
      }
    })
      .then(response => {
        return response.json()
      })
      .then((data) => {
        console.log(data.COUNT);
        const iterationCount = Math.ceil(parseInt(data) / 100);
        for (let i = 0; i < 1; i++) {
          const currentPage = i + 1;
          const url = `https://unogs-unogs-v1.p.rapidapi.com/aaapi.cgi?q=get%3Anew99999%3APL&p=${currentPage}&t=ns&st=adv`;

          fetch(url, {
            "method": "GET",
            "headers": {
              "x-rapidapi-host": "unogs-unogs-v1.p.rapidapi.com",
              "x-rapidapi-key": "8a5735bcd6msh35b94dd1467c587p1baf48jsn33eb07d88120"
            }
          })
            .then(response => {
              return response.json()
            })
            .then((data) => {

              for (let i = 0; i < data.ITEMS.length; i++) {
                if (data.ITEMS[i].imdbid !== "notfound") {
                  const apiData = data.ITEMS[i];
                  const movieDburl = `https://api.themoviedb.org/3/find/${apiData.imdbid}?api_key=61a4454e6812a635ebe4b24f2af2c479&language=pl_PL&external_source=imdb_id`;

                  fetch(movieDburl)
                    .then((response) => {
                      return response.json();
                    })
                    .then((data) => {
                      console.log(data);

                      if (data.movie_results.length > 0) {
                        const movieArray = data.movie_results[0];

                        const categories = movieArray.genre_ids.map(el =>
                          this.categoryList.find(x => x.genreApiId === el)
                        );
                        this.productionFromApi.imbdId = apiData.imdbid
                        this.productionFromApi.poster = apiData.image;
                        //this.productionFromApi.imbdRating = apiData.rating;
                        //this.productionFromApi.title = movieArray.title;
                        this.productionFromApi.title = apiData.title;
                        if (movieArray.overview)
                          this.productionFromApi.synopsis = movieArray.overview;
                        this.productionFromApi.type = 0;
                        this.productionFromApi.releaseDate = new Date(movieArray.release_date);
                        this.productionFromApi.categories = categories;
                        this.productionFromApi.vods = [{
                          vodId: 1,
                          platformName: "Netflix"
                        }];
                      }
                      else if (data.tv_results.length > 0) {
                        const seriesArray = data.tv_results[0];


                        const categories = seriesArray.genre_ids.map(el =>
                          this.categoryList.find(x => x.genreApiId === el)
                        );

                        this.productionFromApi.imbdId = apiData.imdbid
                        this.productionFromApi.poster = apiData.image;
                        //this.productionFromApi.imbdRating = apiData.rating;
                        //this.productionFromApi.title = seriesArray.name;
                        this.productionFromApi.title = apiData.title;
                        if (seriesArray.overview)
                          this.productionFromApi.synopsis = seriesArray.overview;
                        this.productionFromApi.type = 1;
                        this.productionFromApi.releaseDate = new Date(seriesArray.first_air_date);
                        this.productionFromApi.categories = categories;
                        this.productionFromApi.vods = [{
                          vodId: 1,
                          platformName: "Netflix"
                        }];
                      }

                      //
                      const rapidApiUrl = "https://imdb-internet-movie-database-unofficial.p.rapidapi.com/film/" + apiData.imdbid;
                      const imbdResponse = this.getImbdRating(apiData.imdbid);
                      //this.productionFromApi.imbdRating = parseFloat(imbdResponse.rating);
                      // fetch(rapidApiUrl, {
                      //   "method": "GET",
                      //   "headers": {
                      //     "x-rapidapi-host": "imdb-internet-movie-database-unofficial.p.rapidapi.com",
                      //     "x-rapidapi-key": "8a5735bcd6msh35b94dd1467c587p1baf48jsn33eb07d88120"
                      //   }
                      // })
                      //   .then(response => {

                      //     return response.json()
                      //   })
                      //   .then((data) => {
                      //     console.log(data);
                      //     this.productionFromApi.imbdRating = parseFloat(data.rating);

                      //     this.fetchedProductionList.push(this.productionFromApi);

                      //   })
                      //   .catch(err => {
                      //     console.log(err);
                      //   });




                    })
                    .catch(err => {
                      console.log(err);
                    });

                }
              }
            })
            .catch(err => {
              console.log(err);
            });
        }

      })
      .catch(err => {
        console.log(err);
      });
  }

  async getImbdRating(imbdId) {
    const response = await fetch("https://imdb-internet-movie-database-unofficial.p.rapidapi.com/film/" + imbdId, {
      "method": "GET",
      "headers": {
        "x-rapidapi-host": "imdb-internet-movie-database-unofficial.p.rapidapi.com",
        "x-rapidapi-key": "8a5735bcd6msh35b94dd1467c587p1baf48jsn33eb07d88120"
      }
    })
    return await response.json();

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


}
