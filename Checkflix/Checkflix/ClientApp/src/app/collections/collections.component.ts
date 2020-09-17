import { IFollowingCountViewModel } from './../ClientViewModels/IFollowingCountViewModel';
import { FollowingService } from './../../services/following.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-collections',
  templateUrl: './collections.component.html',
  styleUrls: ['./collections.component.css']
})
export class CollectionsComponent implements OnInit {
  followingCount:IFollowingCountViewModel;
  constructor(private followingService:FollowingService) { }

  ngOnInit() {
    console.log("siema")
    this.followingService.getFollowingCount()
    .subscribe(followings => {
      console.log(followings)
      this.followingCount = followings;
    });
  }

}
