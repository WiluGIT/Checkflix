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
  constructor(private followingService:FollowingService,
    @Inject(MAT_DIALOG_DATA) public data) { }

  ngOnInit() {
    this.followingService.getUsers("gowno")
    .subscribe(data=> {
      let a = [];
      a.push(...data);
      a.push(...data);
      a.push(...data);
      a.push(...data);
      a.push(...data);

      console.log(a)
      this.userList = a;

    });
    console.log(this.data)
  }

}
