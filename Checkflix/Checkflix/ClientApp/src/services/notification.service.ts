import { INotificationViewModel } from './../app/ClientViewModels/INotificationViewModel';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private unseenNotificationCountPath = environment.apiUrl + '/api/notifications/GetUnseenNotificationsCount';
  private unseenNotificationPath = environment.apiUrl + '/api/notifications/GetUnseenNotifications';

  constructor(private http: HttpClient) { }

  getUnseenNotificationsCount(): Observable<number> {
    return this.http.get(this.unseenNotificationCountPath).pipe(map((count: number) => count));
  }

  getUnseenNotifications(): Observable<INotificationViewModel[]> {
    return this.http.get(this.unseenNotificationPath).pipe(map((notifications: INotificationViewModel[]) => notifications));
  }
}