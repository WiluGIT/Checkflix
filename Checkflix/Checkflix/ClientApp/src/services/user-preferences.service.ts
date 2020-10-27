import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { IUserPreferencesViewModel } from 'src/app/ClientViewModels/IUserPreferencesViewModel';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserPreferencesService {
  private getPreferencesPath = environment.apiUrl + '/api/preferences/GetUserPreferences';
  constructor(private http: HttpClient) { }

  getPreferences(): Observable<IUserPreferencesViewModel> {
    return this.http.get(this.getPreferencesPath).pipe(map((preferences: IUserPreferencesViewModel) => preferences));
  }
}
