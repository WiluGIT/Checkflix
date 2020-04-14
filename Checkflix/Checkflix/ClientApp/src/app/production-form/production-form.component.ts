import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormBuilder, FormGroup } from '@angular/forms';
import { IProductionViewModel } from '../ClientViewModels/IProductionViewModel';
import { ProductionService } from '../../services/production.service';


@Component({
  selector: 'app-production-form',
  templateUrl: './production-form.component.html',
  styleUrls: ['./production-form.component.css']
})
export class ProductionFormComponent implements OnInit {
  productionForm: FormGroup;
  production: IProductionViewModel;
  imbdFetchClicked: boolean = false;
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

  constructor(private fb: FormBuilder, private productionService: ProductionService) {

  }

  ngOnInit() {
    this.productionForm = this.fb.group({
      title: ['', [
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
      ]]

    });
    //this.productionForm.valueChanges.subscribe(console.log)

    
  }
  get title() {
    return this.productionForm.get('title');
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
    this.production = this.productionForm.value;
    this.production.imbdRating = parseFloat(this.productionForm.value.imbdRating);
    this.production.type = parseInt(this.productionForm.value.type);
    this.production.releaseDate = new Date(this.productionForm.value.releaseDate);

    this.productionService.createProduction(this.production).subscribe(res => console.log(res));

  }

  fetchImbd() {
    const imbdId = this.productionForm.value.imbdId;
    if (imbdId) {

      const movieDburl = `https://api.themoviedb.org/3/find/${imbdId}?api_key=61a4454e6812a635ebe4b24f2af2c479&language=pl_PL&external_source=imdb_id`;
      fetch(movieDburl)
        .then((response) => {
          return response.json();
        })
        .then((data) => {
          console.log(data);

          if (data.movie_results.length > 0) {
            const movieArray = data.movie_results[0];

            this.productionForm.controls.title.setValue(movieArray.title);
            this.productionForm.controls.synopsis.setValue(movieArray.overview);
            this.productionForm.controls.type.setValue(0);
            //this.productionForm.controls.type.setValue(this.typeSelect[0].id);

            this.productionForm.controls.releaseDate.setValue(new Date(movieArray.release_date));
          }
          else if (data.tv_results.length > 0) {
            const seriesArray = data.tv_results[0];
            this.productionForm.controls.title.setValue(seriesArray.name);
            this.productionForm.controls.synopsis.setValue(seriesArray.overview);
            this.productionForm.controls.type.setValue(1);
            this.productionForm.controls.releaseDate.setValue(new Date(seriesArray.first_air_date));
          }


          const rapidApiUrl = "https://imdb-internet-movie-database-unofficial.p.rapidapi.com/film/" + imbdId;
          fetch(rapidApiUrl, {
            "method": "GET",
            "headers": {
              "x-rapidapi-host": "imdb-internet-movie-database-unofficial.p.rapidapi.com",
              "x-rapidapi-key": "8a5735bcd6msh35b94dd1467c587p1baf48jsn33eb07d88120"
            }
          })
            .then(response => {
              return response.json()
            })
            .then((data) => {
              console.log(data);
              this.productionForm.controls.imbdRating.setValue(data.rating);
              this.productionForm.controls.poster.setValue(data.poster);

            })
            .catch(err => {
              console.log(err);
            });

        })
        .catch(err => {
          console.log(err);
        });

    }
    else {
      this.imbdFetchClicked = true;
      //zmiana field na zly stan w celu wyswietlenia wiadomosci
      this.productionForm.controls.imbdId.touched;
    }
    

  }

  getErrorMessage() {
    if (this.imbdFetchClicked) 
      return "Wprowadź Imbd id, aby pobrać dane";
      

  }



}