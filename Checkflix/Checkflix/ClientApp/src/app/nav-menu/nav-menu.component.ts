import { NotificationsComponent } from './../notifications/notifications.component';
import { Component } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(private dialog: MatDialog){}
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

    });
  }
}
