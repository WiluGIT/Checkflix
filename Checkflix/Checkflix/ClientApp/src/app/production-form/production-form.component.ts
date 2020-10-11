import { CategoryService } from './../../services/category.service';
import { VodService } from './../../services/vod.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormBuilder, FormGroup } from '@angular/forms';
import { IProductionViewModel } from '../ClientViewModels/IProductionViewModel';
import { ProductionService } from '../../services/production.service';
import { ActivatedRoute } from '@angular/router';
import { ICategoryViewModel } from '../ClientViewModels/ICategoryViewModel';
import { IVodViewModel } from '../ClientViewModels/IVodViewModel';
import { HttpClient } from '@angular/common/http';
import { MatSnackBar } from '@angular/material';


@Component({
  selector: 'app-production-form',
  templateUrl: './production-form.component.html',
  styleUrls: ['./production-form.component.css']
})
export class ProductionFormComponent implements OnInit {
  productionForm: FormGroup;
  production: IProductionViewModel;
  categoryList: ICategoryViewModel[];
  vodList: IVodViewModel[];
  productionId: number;
  imbdFetchClicked: boolean = false;

  actionText: string;
  typeSelect: any = [
    {
      id: 0,
      value: "Film"
    },
    {
      id: 1,
      value: "Serial"
    }
  ];

  constructor(private fb: FormBuilder,
    private productionService: ProductionService,
    private vodService:VodService,
    private categorySevice:CategoryService,
    private route: ActivatedRoute,
    private http: HttpClient,
    public snackBar: MatSnackBar) {
    route.params.subscribe(p => {
      //+before p converts id to a number
      this.productionId = +p['id'] || null;
    });

    this.actionText = "Stwórz";
  }

  ngOnInit() {
    this.productionForm = this.fb.group({
      title: ['', [
        Validators.required
      ]],
      subtitle: ['', [
        Validators.required
      ]],
      poster: ['', [
        Validators.required
      ]],
      synopsis: ['',
        [Validators.required
        ]],
      type: ['', [
        Validators.required
      ]],
      releaseDate: [new Date(), [
        Validators.required
      ]],
      imbdId: ['', [
        Validators.required
      ]],
      imbdRating: ['', [
        Validators.required,
        Validators.min(0),
        Validators.max(10),
        Validators.minLength(1)
      ]],
      categories: ['', [
        Validators.required
      ]],
      vods: ['', [
        Validators.required
      ]]
    });
    // populate vods and categories to select boxes
    this.vodService
      .getVods()
      .subscribe(vods => this.vodList = vods);

    this.categorySevice
      .getCategories()
      .subscribe(categories => this.categoryList = categories);

    // if its update populate form and 
    if (this.productionId) {
      this.actionText = "Aktualizuj";
      this.productionService.getProduction(this.productionId)
        .subscribe(production => {
          //populate production
          this.production = production;
          //populate form 
          this.productionForm.controls.title.setValue(production.title);
          this.productionForm.controls.poster.setValue(production.poster);
          this.productionForm.controls.synopsis.setValue(production.synopsis);
          this.productionForm.controls.type.setValue(production.type);
          this.productionForm.controls.releaseDate.setValue(production.releaseDate);
          this.productionForm.controls.imbdId.setValue(production.imbdId);
          this.productionForm.controls.imbdRating.setValue(production.imbdRating);
          this.productionForm.controls.subtitle.setValue(production.subtitle);

          // populate dropdown lists
          const selectedCategories = production.categories.map(cat => cat.categoryName);
          const selectedVods = production.vods.map(vod => vod.platformName);
          this.productionForm.controls.categories.setValue(selectedCategories);
          this.productionForm.controls.vods.setValue(selectedVods);
        });
    }
    
  }

  get title() {
    return this.productionForm.get('title');
  }
  get subtitle() {
    return this.productionForm.get('subtitle');
  }
  get poster() {
    return this.productionForm.get('poster');
  }
  get synopsis() {
    return this.productionForm.get('synopsis');
  }
  get type() {
    return this.productionForm.get('type');
  }
  get releaseDate() {
    return this.productionForm.get('releaseDate');
  }
  get imbdId() {
    return this.productionForm.get('imbdId');
  }
  get imbdRating() {
    return this.productionForm.get('imbdRating');
  }

  submitForm() {
    // convert categories, vods dropdownlist value to proper object and set to form value
    const categories = this.productionForm.controls.categories.value.map(el =>
      this.categoryList.find(x => x.categoryName === el)
    );
    const vods = this.productionForm.controls.vods.value.map(el =>
      this.vodList.find(x => x.platformName === el)
    );

    this.productionForm.controls.categories.setValue(categories);
    this.productionForm.controls.vods.setValue(vods);


    // bind values from form to API POST/PUT
    this.production = this.productionForm.value;
    this.production.imbdRating = parseFloat(this.productionForm.value.imbdRating);
    this.production.type = parseInt(this.productionForm.value.type);
    this.production.releaseDate = new Date(this.productionForm.value.releaseDate);

    // if its update (id is in url)
    if (this.productionId) {
      this.production.productionId = this.productionId;
      this.productionService.updateProduction(this.productionId, this.production).subscribe(res => {
        if (res['status'] == 1) {
          this.openSnackBar(res['messages'], 'Zamknij', 'red-snackbar');
        } else {
          this.openSnackBar(res['messages'], 'Zamknij', 'green-snackbar');
        }
      });

    } else {
      this.productionService.createProduction(this.production).subscribe(res => {
        if (res['status'] == 1) {
          this.openSnackBar(res['messages'], 'Zamknij', 'red-snackbar');
        } else {
          this.openSnackBar(res['messages'], 'Zamknij', 'green-snackbar');
        }
      });
    }


    // populate dropdown lists back 
    const selectedCategories = this.production.categories.map(cat => cat.categoryName);
    const selectedVods = this.production.vods.map(vod => vod.platformName);
    this.productionForm.controls.categories.setValue(selectedCategories);
    this.productionForm.controls.vods.setValue(selectedVods);
  }

  async fetchImbd() {
    const imbdId = this.productionForm.value.imbdId;

    if (imbdId) {

      try {
        // themoviedb endpoint 
        const movieDburl = `https://api.themoviedb.org/3/find/${imbdId}?api_key=61a4454e6812a635ebe4b24f2af2c479&language=pl_PL&external_source=imdb_id`;
        let movieDbData = await this.http.get(movieDburl).toPromise();

        // imbd endpoint
        const imbdUrl = "https://imdb-internet-movie-database-unofficial.p.rapidapi.com/film/" + imbdId;
        let imbdData = await this.http.get(imbdUrl, {
          headers: {
            'x-rapidapi-host': 'imdb-internet-movie-database-unofficial.p.rapidapi.com',
            'x-rapidapi-key': '8a5735bcd6msh35b94dd1467c587p1baf48jsn33eb07d88120'
          }
        }).toPromise();


        if (movieDbData["movie_results"].length > 0) {
          const movieArray = movieDbData["movie_results"][0];

          const categories = movieArray.genre_ids.map(el =>
            this.categoryList.find(x => x.genreApiId === el).categoryName
          );

          this.productionForm.controls.categories.setValue(categories);
          this.productionForm.controls.title.setValue(movieArray.title);
          this.productionForm.controls.synopsis.setValue(movieArray.overview);
          this.productionForm.controls.type.setValue(0);


          this.productionForm.controls.releaseDate.setValue(new Date(movieArray.release_date));
        }
        else if (movieDbData["tv_results"].length > 0) {
          const seriesArray = movieDbData["tv_results"][0];


          const categories = seriesArray.genre_ids.map(el =>
            this.categoryList.find(x => x.genreApiId === el).categoryName
          );


          this.productionForm.controls.categories.setValue(categories);
          this.productionForm.controls.title.setValue(seriesArray.name);
          this.productionForm.controls.synopsis.setValue(seriesArray.overview);
          this.productionForm.controls.type.setValue(1);
          this.productionForm.controls.releaseDate.setValue(new Date(seriesArray.first_air_date));
        }

        if (imbdData) {
          this.productionForm.controls.imbdRating.setValue(imbdData["rating"]);
          this.productionForm.controls.poster.setValue(imbdData["poster"]);
          this.productionForm.controls.subtitle.setValue(imbdData["title"]);
        }

        this.openSnackBar('Dane zostały pobrane', 'Zamknij', 'green-snackbar');
      } catch (err) {
        this.openSnackBar("Wystąpił błąd w pobieraniu danych", 'Zamknij', 'red-snackbar')
      }

    } else {
      this.imbdFetchClicked = true;
      this.productionForm.controls.imbdId.markAsTouched();
    }

  }

  getErrorMessage() {
    if (this.imbdFetchClicked) 
      return "Wprowadź Imbd id, aby pobrać dane";
  }

  openSnackBar(message: string, action: string, className: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
      panelClass: [className]
    });
  }
}
