import { LoginMenuComponent } from './../../api-authorization/login-menu/login-menu.component';
import { IUserViewModel } from './../ClientViewModels/IUserViewModel';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { IFollowingCountViewModel } from './../ClientViewModels/IFollowingCountViewModel';
import { FollowingService } from './../../services/following.service';
import { Component, OnInit } from '@angular/core';
import { ApplicationPaths } from 'src/api-authorization/api-authorization.constants';

@Component({
  selector: 'app-collections',
  templateUrl: './collections.component.html',
  styleUrls: ['./collections.component.css']
})
export class CollectionsComponent implements OnInit {
  userFilterForm: FormGroup;
  followingCount:IFollowingCountViewModel;
  userList: IUserViewModel[];
  showDropdown:boolean = false;
  lastKeypress: number = 0;
  constructor(
    private followingService:FollowingService,
    private router: Router,
    private fb: FormBuilder) { }

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

  searchUsers($event) {
    let userSearchValue = this.userFilterForm.controls["searchQuery"].value;
    if($event.timeStamp - this.lastKeypress > 200) {
      this.followingService.getUsers(userSearchValue)
      .subscribe(users => {
        this.userList = users;
      });
    }
    this.lastKeypress = $event.timeStamp;
    console.log(this.lastKeypress)
  }

  handleAuthorization(isAuthenticated: boolean) {
    if (!isAuthenticated) {
      this.router.navigate(ApplicationPaths.LoginPathComponents)
    };
  }
}
