import { NotificationService } from './../../services/notification.service';
import { NotificationsComponent } from './../notifications/notifications.component';
import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit{
  isExpanded = false;
  notificationCount: number;
  constructor(private dialog: MatDialog,
    private notificationService: NotificationService){}

  ngOnInit() {
    this.getNotificationCount();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  showNotifications(){
    const dialogConfig = new MatDialogConfig();

    dialogConfig.autoFocus = false;
    const dialogRef = this.dialog.open(NotificationsComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(result => {
      this.notificationService.markAsSeen()
      .subscribe(result => {
        console.log(result)
        if (result["status"] == 0) {
          this.getNotificationCount();
        }
      });
    });
  }

  getNotificationCount() {
    this.notificationService.getUnseenNotificationsCount()
    .subscribe(count => {
      this.notificationCount = count;
    });
  }
}
