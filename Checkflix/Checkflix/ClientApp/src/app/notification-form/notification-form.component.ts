import { MatSnackBar } from '@angular/material';
import { NotificationService } from './../../services/notification.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder, FormControl, ValidatorFn, ValidationErrors } from '@angular/forms';
import { ICategoryViewModel } from '../ClientViewModels/ICategoryViewModel';
import { IVodViewModel } from '../ClientViewModels/IVodViewModel';
import { VodService } from 'src/services/vod.service';
import { CategoryService } from 'src/services/category.service';
import { INotificationFromViewModel } from '../ClientViewModels/INotificationFormViewModel';
import { ApplicationPaths } from '../../api-authorization/api-authorization.constants';
import { Router } from '@angular/router';

@Component({
  selector: 'app-notification-form',
  templateUrl: './notification-form.component.html',
  styleUrls: ['./notification-form.component.css']
})
export class NotificationFormComponent implements OnInit {
  notificationForm: FormGroup;
  categoryList: ICategoryViewModel[];
  vodList: IVodViewModel[];
  notification: INotificationFromViewModel;
  toAllChecked: boolean = true;
  constructor(private fb: FormBuilder,
    private vodService: VodService,
    private categorySevice: CategoryService,
    private notificationService: NotificationService,
    public snackBar: MatSnackBar,
    private router: Router) { }

  ngOnInit() {
    this.notificationForm = this.fb.group({
      content: ['', [
        Validators.required
      ]],
      categories: [[], [
      ]],
      vods: [[], [
      ]],
      toAll: [this.toAllChecked, [
      ]]
    });

    this.vodService
      .getVods()
      .subscribe(vods => this.vodList = vods);

    this.categorySevice
      .getCategories()
      .subscribe(categories => this.categoryList = categories);

    this.toAll.valueChanges.subscribe(checked => {
      if (checked) {
        // this.notificationForm.controls.categories.setErrors(null);
        // this.notificationForm.controls.vods.setErrors(null);
        // this.notificationForm.setErrors(null)
        this.notificationForm.setValidators(null)
        this.notificationForm.updateValueAndValidity();
      } 
      else {
        this.notificationForm.setValidators(atLeastOne(Validators.required, ['categories','vods']))
        this.notificationForm.updateValueAndValidity();
      }
    });

  }

  get content() {
    return this.notificationForm.get('content');
  }

  get toAll() {
    return this.notificationForm.get('toAll') as FormControl;
  }

  checkCheckbox() {
    this.toAllChecked = !this.toAllChecked;
  }

  submitForm() {
    if (!this.toAllChecked) {
      const categories = this.notificationForm.controls.categories.value.map(el =>
        this.categoryList.find(x => x.categoryName === el)
      );
      this.notificationForm.controls.categories.setValue(categories);
      
      const vods = this.notificationForm.controls.vods.value.map(el =>
        this.vodList.find(x => x.platformName === el)
      );   
      this.notificationForm.controls.vods.setValue(vods);

      this.notification = this.notificationForm.value;

      // populate dropdown lists back 
      const selectedCategories = this.notification.categories.map(cat => cat.categoryName);
      const selectedVods = this.notification.vods.map(vod => vod.platformName);
      this.notificationForm.controls.categories.setValue(selectedCategories);
      this.notificationForm.controls.vods.setValue(selectedVods);
    }
    else {
      this.notificationForm.controls.vods.setValue([]);
      this.notificationForm.controls.categories.setValue([]);
      this.notification = this.notificationForm.value;
    }
    this.notification.date = new Date();

    this.notificationService.createNotification(this.notification).subscribe(res => {
      if (res['status'] == 1) {
        this.openSnackBar(res['messages'], 'Zamknij', 'red-snackbar');
      } else {
        this.openSnackBar(res['messages'], 'Zamknij', 'green-snackbar');
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

  openSnackBar(message: string, action: string, className: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
      panelClass: [className]
    });
  }

  handleAuthorization(isAuthenticated: boolean) {
    if (!isAuthenticated) {
      this.router.navigate(ApplicationPaths.LoginPathComponents)
    };
  }
}



export const atLeastOne = (validator: ValidatorFn, controls:string[] = null) => (
  group: FormGroup,
): ValidationErrors | null => {
  if(!controls){
    controls = Object.keys(group.controls)
  }

  const hasAtLeastOne = group && group.controls && controls
    .some(k => !validator(group.controls[k]));

  return hasAtLeastOne ? null : {
    atLeastOne: true,
  };
};
