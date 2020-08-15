import { IProductionViewModel } from 'src/app/ClientViewModels/IProductionViewModel';
import { ProductionService } from './../../services/production.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-production',
  templateUrl: './production.component.html',
  styleUrls: ['./production.component.css']
})
export class ProductionComponent implements OnInit {
  productionId: number;
  vodId: number;
  production:IProductionViewModel;
  productionType:string;

  constructor(
    private route: ActivatedRoute,
    private productionService: ProductionService) { 
    route.params.subscribe(p => {
      this.productionId = +p['id'] || null;
    });
  }

  ngOnInit() {
    this.productionService.getProduction(this.productionId)
    .subscribe(production =>{
      this.production = production;
    });
  }

}
