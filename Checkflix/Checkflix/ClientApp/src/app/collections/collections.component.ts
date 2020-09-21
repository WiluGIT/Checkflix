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
  userList: any = ["siema","elo","siema"];
  showDropdown:boolean = false;
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

  searchUsers() {
    let userSearchValue = this.userFilterForm.controls["searchQuery"].value;
    console.log(userSearchValue)
  }

  handleAuthorization(isAuthenticated: boolean) {
    if (!isAuthenticated) {
      this.router.navigate(ApplicationPaths.LoginPathComponents)
    };
  }
}
