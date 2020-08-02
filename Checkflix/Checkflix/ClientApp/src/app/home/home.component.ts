import { FormGroup, FormBuilder, NumberValueAccessor } from '@angular/forms';
import { IPostQueryFilters } from './../ClientViewModels/IPostQueryFilters';
import { ProductionService } from './../../services/production.service';
import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { IProductionViewModel } from '../ClientViewModels/IProductionViewModel';
import { MatPaginator } from '@angular/material';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {
  productionList: Array<IProductionViewModel>;
  productionsCount: number;
  postQueryFilters: IPostQueryFilters = {
    pageNumber: 1,
    pageSize: 25,
    searchQuery: null
  };
  activePageDataChunk: Array<IProductionViewModel> = [];
  // Filter form
  productionsFilterForm: FormGroup;

  //Child component slider
  minValueYear:number;
  maxValueYear:number;

  @ViewChild('homePaginator', {static: false}) paginator: MatPaginator;
  constructor(
    private productionService: ProductionService,
    private fb: FormBuilder) {
  }

  ngOnInit() {
    //Endpoint calls
    this.productionService.getProductions(this.postQueryFilters)
      .subscribe(response => {
        const headers = JSON.parse(response.headers.get('X-Pagination'));

        this.productionList = response.body;
        this.productionsCount = headers["TotalCount"];
        this.activePageDataChunk = this.productionList;
      });

    this.productionsFilterForm = this.fb.group({
      searchQuery: ''
    });

    //Set variables
    this.maxValueYear = new Date().getFullYear();
    this.minValueYear = 1900;
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

    // Change params to default
    this.postQueryFilters.pageNumber = 1;
    this.postQueryFilters.pageSize = 25;
    //Change page to first
    this.paginator.pageIndex = 0;

    this.productionService.getProductions(this.postQueryFilters)
      .subscribe(response => {
        const headers = JSON.parse(response.headers.get('X-Pagination'));

        this.productionList = response.body;
        this.productionsCount = headers["TotalCount"];
        this.activePageDataChunk = this.productionList;
      });

    // Reset fields
    this.postQueryFilters.searchQuery = null;
  }
}
