import { IUserPreferencesViewModel } from './../app/ClientViewModels/IUserPreferencesViewModel';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserPreferencesService {
  private getPreferencesPath = environment.apiUrl + '/api/preferences/GetUserPreferences';
  private putPreferencesPath = environment.apiUrl + '/api/preferences/UpdatePreferences';

  constructor(private http: HttpClient) { }

  getPreferences(): Observable<IUserPreferencesViewModel> {
    return this.http.get(this.getPreferencesPath).pipe(map((preferences: IUserPreferencesViewModel) => preferences));
  }

  updatePreferences(userPreferences: IUserPreferencesViewModel) {
    return this.http.put(this.putPreferencesPath, userPreferences).pipe(map((preferences: IUserPreferencesViewModel) => preferences));
  }
}
