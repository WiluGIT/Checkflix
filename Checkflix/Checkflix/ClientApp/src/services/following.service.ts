import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IFollowingCountViewModel } from 'src/app/ClientViewModels/IFollowingCountViewModel';
import { map } from 'rxjs/internal/operators/map';

@Injectable({
  providedIn: 'root'
})
export class FollowingService {
  private getCategoriesPath = environment.apiUrl + '/api/followings/GetFollowingCount';
  constructor(private http: HttpClient) { }

    // Categories
    getFollowingCount(): Observable<IFollowingCountViewModel> {
      return this.http.get(this.getCategoriesPath).pipe(map((followingCount: IFollowingCountViewModel) => followingCount));
    }
}
