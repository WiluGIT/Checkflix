import { IProductionViewModel } from 'src/app/ClientViewModels/IProductionViewModel';
import { IUserCollectionFilter } from './../app/ClientViewModels/IUserCollectionFilter';
import { IApplicationUserProductionViewModel } from './../app/ClientViewModels/IApplicationUserProductionViewModel';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/internal/operators/map';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class UserProducionService {
  private addUserProductionPath = environment.apiUrl + '/api/userproductions/postuserproduction';
  private getUserProductionsPath = environment.apiUrl + '/api/userproductions/getuserproductions';
  private getUserCollectionPath = environment.apiUrl + '/api/userproductions/getusercollection';

  constructor(private http: HttpClient) { }

  addUserProduction(userProduction: IApplicationUserProductionViewModel) {
    return this.http.post(this.addUserProductionPath, userProduction).pipe(map((production: any) => production));
  }

  getUserProductions(){
    return this.http.get(this.getUserProductionsPath).pipe(map((productions:IApplicationUserProductionViewModel[]) => productions));
  }

  getUserCollection(userQueryFilters: IUserCollectionFilter) {
    let params = new HttpParams();
    params = params.append('pageNumber', userQueryFilters.pageNumber.toString());
    params = params.append('pageSize', userQueryFilters.pageSize.toString());
    if (userQueryFilters.userId)
      params = params.append('userId', userQueryFilters.userId.toString());
    if (userQueryFilters.favourites)
      params = params.append('favourites', userQueryFilters.favourites.toString());
    else if (userQueryFilters.toWatch)
      params = params.append('toWatch', userQueryFilters.toWatch.toString());
    else if (userQueryFilters.watched)
      params = params.append('watched', userQueryFilters.watched.toString());

    return this.http.get(this.getUserCollectionPath, { params, observe: 'response' }).pipe(map((productions:any) => productions));
  }
}
