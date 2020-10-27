import { IUserViewModel } from './../app/ClientViewModels/IUserViewModel';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IFollowingCountViewModel } from 'src/app/ClientViewModels/IFollowingCountViewModel';
import { map } from 'rxjs/internal/operators/map';

@Injectable({
  providedIn: 'root'
})
export class FollowingService {
  private getCategoriesPath = environment.apiUrl + '/api/followings/GetFollowingCount';
  private getUsersPath = environment.apiUrl + '/api/followings/GetUsers';
  private getUserPath = environment.apiUrl + '/api/followings/GetUser';
  private checkFollowingPath = environment.apiUrl + '/api/followings/CheckIfFollowing';
  private getFollowersPath = environment.apiUrl + '/api/followings/GetFollowers';
  private getFolloweesPath = environment.apiUrl + '/api/followings/GetFollowees';
  private postFollowingPath = environment.apiUrl + '/api/followings/PostFollowing';
  private muteUserPath = environment.apiUrl + '/api/followings/MuteFollowee';
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

  getUser(userId:string):Observable<IUserViewModel> {
    let params = new HttpParams();
    if (userId)
      params = params.append('userId', userId.toString());
    return this.http.get(this.getUserPath, { params }).pipe(map((user: IUserViewModel) => user));
  }

  checkIfFollowing(userId:string):Observable<boolean> {
    let params = new HttpParams();
    if (userId)
      params = params.append('userId', userId.toString());
    return this.http.get(this.checkFollowingPath, { params }).pipe(map((res: boolean) => res));
  }

  getFollowers(): Observable<IUserViewModel[]> {
    return this.http.get(this.getFollowersPath).pipe(map((users: IUserViewModel[]) => users));
  }

  getFollowees(): Observable<IUserViewModel[]> {
    return this.http.get(this.getFolloweesPath).pipe(map((users: IUserViewModel[]) => users));
  }

  postFollowing(followeeId: string) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.post(this.postFollowingPath, JSON.stringify(followeeId), {headers});
  }

  muteUser(followeeId: string) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.put(this.muteUserPath, JSON.stringify(followeeId), {headers});
  }
}
