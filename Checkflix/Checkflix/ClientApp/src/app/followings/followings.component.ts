import { IUserViewModel } from './../ClientViewModels/IUserViewModel';
import { FollowingService } from './../../services/following.service';
import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-followings',
  templateUrl: './followings.component.html',
  styleUrls: ['./followings.component.css']
})
export class FollowingsComponent implements OnInit {
  userList:IUserViewModel[];
  followeesList :IUserViewModel[];
  constructor(private followingService:FollowingService,
    @Inject(MAT_DIALOG_DATA) public data,
    public dialogRef: MatDialogRef<FollowingsComponent>,
    public snackBar:MatSnackBar) { }

  ngOnInit() {
    if(this.data.type == 1) {
      this.followingService.getFollowers()
      .subscribe(users => {
        this.userList = users;

        this.followingService.getFollowees()
        .subscribe(users => {
          this.followeesList = users;
        });
      });
    }
    else if (this.data.type == 2) {
      this.followingService.getFollowees()
      .subscribe(users => {
        this.userList = users;
      });
    }
  }

  followingClick(followeId,e) {
    const targetButton = e.currentTarget;
    this.followingService.postFollowing(followeId)
    .subscribe(response => {
      if (response["status"] == 0) {
        targetButton.classList.add("following");
        targetButton.textContent = "Obserwujesz"
      }
      else if (response["status"] == 1) {
        targetButton.classList.remove("following");
        targetButton.textContent = "Obserwuj";
      }
    });
  }

  checkIfFollowing(userId) {
    if (this.followeesList && this.userList) {
      return this.followeesList.some(el => el.id == userId);
    }
  }

  muteUser(userId, e) {
    const targetButton = e.currentTarget;
    this.followingService.muteUser(userId)
    .subscribe(response => {
      if (response["status"] == 0) {
        targetButton.classList.add("mute-btn");
        targetButton.classList.remove("mute-btn-y");
        this.openSnackBar(response['messages'], 'Zamknij', 'green-snackbar');
      }
      else if (response["status"] == 1) {
        targetButton.classList.remove("mute-btn");
        targetButton.classList.add("mute-btn-y");
        this.openSnackBar(response['messages'], 'Zamknij', 'gray-snackbar');
      }
    });   
  }


  openSnackBar(message: string, action: string, className: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
      panelClass: [className]
    });
  }
}
