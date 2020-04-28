import { ProductionService } from './../../services/production.service';
import { Component, OnInit } from '@angular/core';
import { IProductionViewModel } from '../ClientViewModels/IProductionViewModel';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  productionList: Array<IProductionViewModel>;
  
  constructor(private productionService:ProductionService){}

  ngOnInit(){
    this.productionService.getProductions()
    .subscribe(productions => this.productionList = productions);
  }
}
