import { FormGroup, FormBuilder, NumberValueAccessor } from '@angular/forms';
import { IPostQueryFilters } from './../ClientViewModels/IPostQueryFilters';
import { ProductionService } from './../../services/production.service';
import { Component, OnInit, ViewChild, AfterViewInit, Inject } from '@angular/core';
import { IProductionViewModel } from '../ClientViewModels/IProductionViewModel';
import { MatPaginator } from '@angular/material';
import { DOCUMENT } from '@angular/common';

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
    searchQuery: null,
    isNetflix: null,
    isHbo: null,
    yearFrom: null,
    yearTo: null,
    ratingFrom: null,
    ratingTo: null
  };
  activePageDataChunk: Array<IProductionViewModel> = [];
  // Filter form
  productionsFilterForm: FormGroup;

  //Child component slider
  minValueYear:number;
  maxValueYear:number;
  minImbdRating:number;
  maxImbdRating:number;
  //Vods icons
  netflixClicked:boolean=true;
  hboClicked:boolean=true;

  @ViewChild('homePaginator', {static: false}) paginator: MatPaginator;
  constructor(
    private productionService: ProductionService,
    private fb: FormBuilder,
    @Inject(DOCUMENT) document) {
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
      searchQuery: '',
      isNetflix: null,
      isHbo: null,
      yearFrom: null,
      yearTo: null,
      ratingFrom: null,
      ratingTo: null
    });

    //Set variables
    this.maxValueYear = new Date().getFullYear();
    this.minValueYear = 1900;
    this.minImbdRating = 0;
    this.maxImbdRating = 10;
  }

  getYearSliderValues(valueEmitted){
    this.productionsFilterForm.controls["yearFrom"].setValue(parseInt(valueEmitted['left']));
    this.productionsFilterForm.controls["yearTo"].setValue(parseInt(valueEmitted['right']));
  }

  getImbdSliderValues(valueEmitted){
    this.productionsFilterForm.controls["ratingFrom"].setValue(parseInt(valueEmitted['left']));
    this.productionsFilterForm.controls["ratingTo"].setValue(parseInt(valueEmitted['right']));
  }

  checkBtn(e){
    const target = e.currentTarget;
    if(target.id ==="hbo-btn"){
      if(this.hboClicked){
        target.classList.add("vod-gray");
        target.classList.remove("hbo-btn");
        this.hboClicked = false;
        this.productionsFilterForm.controls["isHbo"].setValue(false);
      }else{
        target.classList.remove("vod-gray");
        target.classList.add("hbo-btn");
        this.hboClicked = true;
        this.productionsFilterForm.controls["isHbo"].setValue(null);
      }
    }else if(target.id === "netflix-btn"){
      if(this.netflixClicked){
        target.classList.add("vod-gray");
        target.classList.remove("netflix-btn");
        this.netflixClicked = false;
        this.productionsFilterForm.controls["isNetflix"].setValue(false);
      }else{
        target.classList.remove("vod-gray");
        target.classList.add("netflix-btn");
        this.netflixClicked = true;
        this.productionsFilterForm.controls["isNetflix"].setValue(null);
      }
    }
    // At least one element must be clicked
    if(!this.hboClicked && !this.netflixClicked){
      this.hboClicked = true;
      this.netflixClicked = true;
      document.getElementById('hbo-btn').classList.remove("vod-gray");
      document.getElementById('netflix-btn').classList.remove("vod-gray");
      document.getElementById('hbo-btn').classList.add("hbo-btn");
      document.getElementById('netflix-btn').classList.add("netflix-btn");
      this.productionsFilterForm.controls["isNetflix"].setValue(null);
      this.productionsFilterForm.controls["isHbo"].setValue(null);
    }
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
    if(this.productionsFilterForm.controls["searchQuery"].value) {
      this.postQueryFilters["searchQuery"] = this.productionsFilterForm.controls["searchQuery"].value;
    }
    if(this.productionsFilterForm.controls["yearFrom"].value > 1900 || this.productionsFilterForm.controls["yearTo"].value < new Date().getFullYear()){
      this.postQueryFilters["yearFrom"] = this.productionsFilterForm.controls["yearFrom"].value;
      this.postQueryFilters["yearTo"] = this.productionsFilterForm.controls["yearTo"].value;
    }
    if(this.productionsFilterForm.controls["ratingFrom"].value > 0 || this.productionsFilterForm.controls["ratingTo"].value < 10){
      this.postQueryFilters["ratingFrom"] = this.productionsFilterForm.controls["ratingFrom"].value;
      this.postQueryFilters["ratingTo"] = this.productionsFilterForm.controls["ratingTo"].value;
    }
    if(this.productionsFilterForm.controls["isHbo"].value == false || this.productionsFilterForm.controls["isNetflix"].value == false){
      this.postQueryFilters["isHbo"] = this.productionsFilterForm.controls["isHbo"].value;
      this.postQueryFilters["isNetflix"] = this.productionsFilterForm.controls["isNetflix"].value;
    }
    console.log(this.postQueryFilters)
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
    //this.postQueryFilters.searchQuery = null;
    this.postQueryFilters = {
      searchQuery: '',
      isNetflix: null,
      isHbo: null,
      yearFrom: null,
      yearTo: null,
      ratingFrom: null,
      ratingTo: null,
      pageNumber: 1,
      pageSize: 25
    };
  }
}
