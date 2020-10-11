import { NotificationService } from './../../services/notification.service';
import { INotificationViewModel } from './../ClientViewModels/INotificationViewModel';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit {
  notificationList: INotificationViewModel[];
  constructor(private notificationService: NotificationService) { }

  ngOnInit() {
    this.notificationService.getUnseenNotifications()
    .subscribe(notifications => {
      this.notificationList = notifications;
    });
  }

  loadNotificationHostory() {
    this.notificationService.getNotifications()
    .subscribe(notifications => {
      this.notificationList = notifications;
    });
  }
}
