import { IUserViewModel } from './../ClientViewModels/IUserViewModel';
import { FollowingService } from './../../services/following.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user-collection',
  templateUrl: './user-collection.component.html',
  styleUrls: ['./user-collection.component.css']
})
export class UserCollectionComponent implements OnInit {
  userId: string;
  user: IUserViewModel;
  following:boolean;
  constructor(private route: ActivatedRoute,
    private followingService: FollowingService) { 
    route.params.subscribe(p => {
      this.userId = p['id'] || null;
    });
  }

  ngOnInit() {
    this.followingService.getUser(this.userId)
    .subscribe(user => {
      this.user = user;
    });

    this.followingService.checkIfFollowing(this.userId)
    .subscribe(followingRes => {
      this.following = followingRes;
    });
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
}
