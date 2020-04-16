import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthorizeService } from '../../api-authorization/authorize.service';
import { map, filter, delay } from 'rxjs/operators';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, of } from 'rxjs';
import { ProductionService } from '../../services/production.service';
import { IProductionViewModel } from '../ClientViewModels/IProductionViewModel';
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
  deletedProduction: IProductionViewModel;

  @ViewChild(MatSort, { static: true }) sort: MatSort;
  constructor(private authService: AuthorizeService, private productionService: ProductionService, private router: Router) {
    this.dataSource = new MatTableDataSource(this.productionList);
  }

  ngOnInit() {
    this.productionService.getProductions()
      .subscribe(productions => this.productionList = productions);

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

}
