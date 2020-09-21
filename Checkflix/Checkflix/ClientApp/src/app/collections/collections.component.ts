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
  followingCount:IFollowingCountViewModel;
  userList: any = ["siema","elo","gowno","beka"];
  showDropdown:boolean = false;
  constructor(
    private followingService:FollowingService,
    private router: Router) { }

  ngOnInit() {
    this.followingService.getFollowingCount()
    .subscribe(followings => {
      this.followingCount = followings;
    }, (err => {
      if (err.status == 401) {
        this.handleAuthorization(false);
      }
    }));
  }

  closeDropDown() {
    this.showDropdown = false;
  }

  openDropDown() {
    this.showDropdown = true;
  }

  handleAuthorization(isAuthenticated: boolean) {
    if (!isAuthenticated) {
      this.router.navigate(ApplicationPaths.LoginPathComponents)
    };
  }
}
