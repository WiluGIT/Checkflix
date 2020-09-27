import { FollowingsComponent } from './../followings/followings.component';
import { IUserViewModel } from './../ClientViewModels/IUserViewModel';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { IFollowingCountViewModel } from './../ClientViewModels/IFollowingCountViewModel';
import { FollowingService } from './../../services/following.service';
import { Component, OnInit, Inject } from '@angular/core';
import { ApplicationPaths } from 'src/api-authorization/api-authorization.constants';
import { MatDialog, MatDialogConfig } from "@angular/material";
@Component({
  selector: 'app-collections',
  templateUrl: './collections.component.html',
  styleUrls: ['./collections.component.css']
})
export class CollectionsComponent implements OnInit {
  userFilterForm: FormGroup;
  followingCount: IFollowingCountViewModel;
  userList: IUserViewModel[];
  showDropdown: boolean = false;
  lastKeypress: number = 0;
  constructor(
    private followingService: FollowingService,
    private router: Router,
    private fb: FormBuilder,
    private dialog: MatDialog) { }

  ngOnInit() {
    this.followingService.getFollowingCount()
      .subscribe(followings => {
        this.followingCount = followings;
      }, (err => {
        if (err.status == 401) {
          this.handleAuthorization(false);
        }
      }));

    this.userList = [];

    this.userFilterForm = this.fb.group({
      searchQuery: null,
    });
  }

  closeDropDown() {
    this.showDropdown = false;
  }

  openDropDown() {
    this.showDropdown = true;
  }

  siema(followeId) {
    this.followingService.postFollowing(followeId)
      .subscribe(res => {
        console.log(res)
      });
  }

  searchUsers($event) {
    let userSearchValue = this.userFilterForm.controls["searchQuery"].value;
    if ($event.timeStamp - this.lastKeypress > 200) {
      this.followingService.getUsers(userSearchValue)
        .subscribe(users => {
          this.userList = users;
        });
    }
    this.lastKeypress = $event.timeStamp;
  }

  showFollowers() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      type: 1,
      collectionName: "Followers"
    };
    dialogConfig.autoFocus = false;
    const dialogRef = this.dialog.open(FollowingsComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(result => {
      this.updateFollowingCount();
    });
  }

  showFollowees() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      type: 2,
      collectionName: "Followees"
    };
    dialogConfig.autoFocus = false;
    const dialogRef = this.dialog.open(FollowingsComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(result => {
      this.updateFollowingCount();
    });
  }

  updateFollowingCount() {
    this.followingService.getFollowingCount()
    .subscribe(followings => {
      this.followingCount = followings;
    }, (err => {
      if (err.status == 401) {
        this.handleAuthorization(false);
      }
    }));
  }

  handleAuthorization(isAuthenticated: boolean) {
    if (!isAuthenticated) {
      this.router.navigate(ApplicationPaths.LoginPathComponents)
    };
  }
}
