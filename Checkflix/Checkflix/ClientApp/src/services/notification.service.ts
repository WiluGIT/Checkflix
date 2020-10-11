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
  private getNotificationPath = environment.apiUrl + '/api/notifications/GetNotifications';
  private markAsSeenPath = environment.apiUrl + '/api/notifications/MarkAsSeen';
  constructor(private http: HttpClient) { }

  getUnseenNotificationsCount(): Observable<number> {
    return this.http.get(this.unseenNotificationCountPath).pipe(map((count: number) => count));
  }

  getUnseenNotifications(): Observable<INotificationViewModel[]> {
    return this.http.get(this.unseenNotificationPath).pipe(map((notifications: INotificationViewModel[]) => notifications));
  }

  getNotifications(): Observable<INotificationViewModel[]> {
    return this.http.get(this.getNotificationPath).pipe(map((notifications: INotificationViewModel[]) => notifications));
  }

  markAsSeen() {
    return this.http.put(this.markAsSeenPath, {});
  }
}
