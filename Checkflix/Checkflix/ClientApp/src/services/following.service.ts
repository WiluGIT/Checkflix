import { IUserViewModel } from './../app/ClientViewModels/IUserViewModel';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IFollowingCountViewModel } from 'src/app/ClientViewModels/IFollowingCountViewModel';
import { map } from 'rxjs/internal/operators/map';

@Injectable({
  providedIn: 'root'
})
export class FollowingService {
  private getCategoriesPath = environment.apiUrl + '/api/followings/GetFollowingCount';
  private getUsersPath = environment.apiUrl + '/api/followings/GetUsers';
  constructor(private http: HttpClient) { }

    // Categories
    getFollowingCount(): Observable<IFollowingCountViewModel> {
      return this.http.get(this.getCategoriesPath).pipe(map((followingCount: IFollowingCountViewModel) => followingCount));
    }

    getUsers(searchQuery: string): Observable<IUserViewModel[]> {
      let params = new HttpParams();
      if (searchQuery)
        params = params.append('searchQuery', searchQuery.toString());
      return this.http.get(this.getUsersPath, { params }).pipe(map((users: IUserViewModel[]) => users));
    }
}
