import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IProductionViewModel } from '../app/ClientViewModels/IProductionViewModel';
import { map } from 'rxjs/operators';
import { ICategoryViewModel } from '../app/ClientViewModels/ICategoryViewModel';
import { IVodViewModel } from '../app/ClientViewModels/IVodViewModel';

@Injectable({
  providedIn: 'root'
})
export class ProductionService {

  constructor(private http: HttpClient) { }

  // Productions
  getProductions(): Observable<IProductionViewModel[]> {
    return this.http.get('/api/productions').pipe(map((productions: IProductionViewModel[]) => productions));
  }

  getProduction(id: number): Observable<IProductionViewModel> {
    return this.http.get('/api/productions/' + id).pipe(map((production: IProductionViewModel) => production));
  }

  createProduction(production: IProductionViewModel) {
    return this.http.post('/api/productions', production).pipe(map((production: IProductionViewModel) => production));
  }

  createProductions(productions: IProductionViewModel[]) {
    return this.http.post('/api/productions/BulkProductionsCreate', productions).pipe(map((productions: IProductionViewModel[]) => productions));
  }

  updateProduction(id:number, production: IProductionViewModel) {
    return this.http.put('/api/productions/'+id, production).pipe(map((production: IProductionViewModel) => production));
  }

  deleteProduction(id: number) {
    return this.http.delete('/api/productions/' + id).pipe(map((production: IProductionViewModel) => production));
  }

  // Categories
  getCategories(): Observable<ICategoryViewModel[]> {
    return this.http.get('/api/categories').pipe(map((categories: ICategoryViewModel[]) => categories));
  }
  updateCategories(categories: ICategoryViewModel[]) {
    return this.http.post('/api/categories', categories).pipe(map((production: IProductionViewModel) => production));

  }

  // Vods
  getVods(): Observable<IVodViewModel[]> {
    return this.http.get('/api/vods').pipe(map((vods: IVodViewModel[]) => vods));
  }
}
