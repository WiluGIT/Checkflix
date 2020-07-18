import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ICategoryViewModel } from 'src/app/ClientViewModels/ICategoryViewModel';
import { map } from 'rxjs/operators';
import { IProductionViewModel } from 'src/app/ClientViewModels/IProductionViewModel';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private getCategoriesPath = environment.apiUrl + '/api/categories/GetCategories';
  constructor(private http: HttpClient) { }

    // Categories
    getCategories(): Observable<ICategoryViewModel[]> {
      return this.http.get(this.getCategoriesPath).pipe(map((categories: ICategoryViewModel[]) => categories));
    }
    updateCategories(categories: ICategoryViewModel[]) {
      return this.http.post('/api/categories', categories).pipe(map((production: IProductionViewModel) => production));
  
    }
}
