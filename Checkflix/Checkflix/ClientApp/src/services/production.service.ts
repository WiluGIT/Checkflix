import { IPostQueryFilters } from './../app/ClientViewModels/IPostQueryFilters';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IProductionViewModel } from '../app/ClientViewModels/IProductionViewModel';
import { map } from 'rxjs/operators';
import { ICategoryViewModel } from '../app/ClientViewModels/ICategoryViewModel';
import { IVodViewModel } from '../app/ClientViewModels/IVodViewModel';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductionService {
  private getProductionsPath = environment.apiUrl + '/api/productions/GetProductions';
  private getProductionPath = environment.apiUrl + '/api/productions/GetProduction/';
  private createProductionPath = environment.apiUrl + '/api/productions/PostProduction';
  private createProductionsPath = environment.apiUrl + '/api/productions/BulkProductionsCreate';
  private updateProductionPath = environment.apiUrl + '/api/productions/PutProduction/';
  private deleteProductionPath = environment.apiUrl + '/api/productions/DeleteProduction/';


  constructor(private http: HttpClient) { }

  // Productions
  getProductions(filters: IPostQueryFilters): Observable<any> {
    let params = new HttpParams();
    params = params.append('pageNumber', filters.pageNumber.toString());
    params = params.append('pageSize', filters.pageSize.toString());
    if (filters.searchQuery)
      params = params.append('searchQuery', filters.searchQuery.toString());
    if (filters.isHbo == false)
      params = params.append('isHbo', filters.isHbo.toString());
    if (filters.isNetflix == false)
      params = params.append('isNetflix', filters.isNetflix.toString());
    if (filters.ratingFrom || filters.ratingFrom === 0)
      params = params.append('ratingFrom', filters.ratingFrom.toString());
    if (filters.ratingTo || filters.ratingFrom === 0)
      params = params.append('ratingTo', filters.ratingTo.toString());
    if (filters.yearFrom)
      params = params.append('yearFrom', filters.yearFrom.toString());
    if (filters.yearTo)
      params = params.append('yearTo', filters.yearTo.toString());



    return this.http.get(this.getProductionsPath, { params, observe: 'response' }).pipe(map((productions: any) => productions));
  }

  getProduction(id: number): Observable<IProductionViewModel> {
    return this.http.get(this.getProductionPath + id).pipe(map((production: IProductionViewModel) => production));
  }

  createProduction(production: IProductionViewModel) {
    return this.http.post(this.createProductionPath, production).pipe(map((production: IProductionViewModel) => production));
  }

  createProductions(productions: IProductionViewModel[]) {
    return this.http.post(this.createProductionsPath, productions).pipe(map((productions: IProductionViewModel[]) => productions));
  }

  updateProduction(id: number, production: IProductionViewModel) {
    return this.http.put(this.updateProductionPath + id, production).pipe(map((production: IProductionViewModel) => production));
  }

  deleteProduction(id: number) {
    return this.http.delete(this.deleteProductionPath + id).pipe(map((production: IProductionViewModel) => production));
  }




}
