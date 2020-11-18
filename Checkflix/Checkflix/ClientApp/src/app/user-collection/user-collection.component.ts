import { IUserViewModel } from './../ClientViewModels/IUserViewModel';
import { FollowingService } from './../../services/following.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApplicationPaths } from '../../api-authorization/api-authorization.constants';
import { MatSnackBar } from '@angular/material';

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
    private router: Router,
    private followingService: FollowingService,
    public snackBar: MatSnackBar) { 
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
        this.openSnackBar("Zaobserwowano", 'Zamknij', 'green-snackbar');
      }
      else if (response["status"] == 1) {
        targetButton.classList.remove("following");
        targetButton.textContent = "Obserwuj";
        this.openSnackBar("Usunięto obserwacje", 'Zamknij', 'gray-snackbar');
      }
    }, (err => {
      if (err.status == 401) {
        this.handleAuthorization(false);
      }
      else {
        this.openSnackBar("Spróbuj ponownie", 'Zamknij', 'red-snackbar');
      }
    })); 
  }

  handleAuthorization(isAuthenticated: boolean) {
    if (!isAuthenticated) {
      this.router.navigate(ApplicationPaths.LoginPathComponents)
    };
  }

  openSnackBar(message: string, action: string, className: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
      panelClass: [className]
    });
  }
}
