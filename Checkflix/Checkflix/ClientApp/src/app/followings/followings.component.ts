import { IUserViewModel } from './../ClientViewModels/IUserViewModel';
import { FollowingService } from './../../services/following.service';
import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-followings',
  templateUrl: './followings.component.html',
  styleUrls: ['./followings.component.css']
})
export class FollowingsComponent implements OnInit {
  userList:IUserViewModel[];
  followeesList :IUserViewModel[];
  constructor(private followingService:FollowingService,
    @Inject(MAT_DIALOG_DATA) public data) { }

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

  followingClick(followeId) {
    this.followingService.postFollowing(followeId)
    .subscribe(res => {
      console.log(res)
    });
  }

  checkIfFollowing(userId) {
    if (this.followeesList && this.userList) {
      return this.followeesList.some(el => el.id == userId);
    }
  }
}
