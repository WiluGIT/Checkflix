import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IProductionViewModel } from '../app/ClientViewModels/IProductionViewModel';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProductionService {

  constructor(private http: HttpClient) { }


  getProductions(): Observable<IProductionViewModel[]> {
    return this.http.get('/api/productions').pipe(map((production: IProductionViewModel[]) => production));
  }

  createProduction(production: IProductionViewModel) {
    return this.http.post('/api/productions', production).pipe(map((production: IProductionViewModel) => production));
  }
}
