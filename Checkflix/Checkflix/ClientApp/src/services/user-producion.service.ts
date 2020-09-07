import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { IApplicationUserProductionViewModel } from 'src/app/ClientViewModels/IApplicationUserProductionViewModel';
import { map } from 'rxjs/internal/operators/map';

@Injectable({
  providedIn: 'root'
})
export class UserProducionService {
  private addUserProductionPath = environment.apiUrl + '/api/userproductions/postuserproduction';

  constructor(private http: HttpClient) { }

  addUserProduction(userProduction: IApplicationUserProductionViewModel) {
    return this.http.post(this.addUserProductionPath, userProduction).pipe(map((production: IApplicationUserProductionViewModel) => production));
  }
}
