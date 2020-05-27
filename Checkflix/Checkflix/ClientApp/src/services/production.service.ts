import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
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
  //to another service
  private getCategoriesPath = environment.apiUrl + '/api/categories/GetCategories';
  private getVodsPath = environment.apiUrl + '/api/vods/GetVods';
  constructor(private http: HttpClient) { }

  // Productions
  getProductions(): Observable<IProductionViewModel[]> {
    return this.http.get(this.getProductionsPath).pipe(map((productions: IProductionViewModel[]) => productions));
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

  updateProduction(id:number, production: IProductionViewModel) {
    return this.http.put(this.updateProductionPath + id, production).pipe(map((production: IProductionViewModel) => production));
  }

  deleteProduction(id: number) {
    return this.http.delete(this.deleteProductionPath + id).pipe(map((production: IProductionViewModel) => production));
  }

  // Categories
  getCategories(): Observable<ICategoryViewModel[]> {
    return this.http.get(this.getCategoriesPath).pipe(map((categories: ICategoryViewModel[]) => categories));
  }
  updateCategories(categories: ICategoryViewModel[]) {
    return this.http.post('/api/categories', categories).pipe(map((production: IProductionViewModel) => production));

  }

  // Vods
  getVods(): Observable<IVodViewModel[]> {
    return this.http.get(this.getVodsPath).pipe(map((vods: IVodViewModel[]) => vods));
  }
}
