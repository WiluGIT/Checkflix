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

  constructor(private fb: FormBuilder, private productionService: ProductionService) { }

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
    this.productionForm.valueChanges.subscribe(console.log)

    // services
    this.productionService.getProductions().subscribe(productionList => this.productionList = productionList);
    
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
    this.production.imbdRating = parseInt(this.productionForm.value.imbdRating);
    this.production.type = parseInt(this.productionForm.value.type);


    this.productionService.createProduction(this.production).subscribe(res => console.log(res));

  }

  fetchImbd() {
    const imbdId = this.productionForm.value.imbdId;
    if (imbdId) {
      const url = "https://imdb-internet-movie-database-unofficial.p.rapidapi.com/film/" + imbdId;
      fetch(url, {
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
          this.productionForm.controls.title.setValue(data.title);
          this.productionForm.controls.releaseDate.setValue(data.year);
          this.productionForm.controls.imbdRating.setValue(data.rating);
          this.productionForm.controls.poster.setValue(data.poster);
          this.productionForm.controls.synopsis.setValue(data.plot);
        })
        .catch(err => {
          console.log(err);
        });
    }
    else {
      this.imbdFetchClicked = true;
      //zmiana field na zly stan w celu wyswietlenia wiadomosci
      this.productionForm.controls.imbdId.invalid;
    }
    

  }

  getErrorMessage() {
    if (this.imbdFetchClicked) 
      return "Wprowadź Imbd id, aby pobrać dane";
      

  }

}
