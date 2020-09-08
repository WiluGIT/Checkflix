import { IApplicationUserProductionViewModel } from './../app/ClientViewModels/IApplicationUserProductionViewModel';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/internal/operators/map';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class UserProducionService {
  private addUserProductionPath = environment.apiUrl + '/api/userproductions/postuserproduction';
  private getUserProductionsPath = environment.apiUrl + '/api/userproductions/getuserproductions';

  constructor(private http: HttpClient) { }

  addUserProduction(userProduction: IApplicationUserProductionViewModel) {
    return this.http.post(this.addUserProductionPath, userProduction).pipe(map((production: any) => production));
  }

  getUserProductions(){
    return this.http.get(this.getUserProductionsPath).pipe(map((productions:IApplicationUserProductionViewModel[]) => productions));
  }
}
