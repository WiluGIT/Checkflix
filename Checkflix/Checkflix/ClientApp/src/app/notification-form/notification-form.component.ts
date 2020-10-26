import { NotificationService } from './../../services/notification.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder, FormControl } from '@angular/forms';
import { ICategoryViewModel } from '../ClientViewModels/ICategoryViewModel';
import { IVodViewModel } from '../ClientViewModels/IVodViewModel';
import { VodService } from 'src/services/vod.service';
import { CategoryService } from 'src/services/category.service';
import { INotificationFromViewModel } from '../ClientViewModels/INotificationFormViewModel';

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
    private notificationService: NotificationService) { }

  ngOnInit() {
    this.notificationForm = this.fb.group({
      content: ['', [
        Validators.required
      ]],
      categories: [null, [
      ]],
      vods: [null, [
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
        this.notificationForm.controls.categories.setErrors(null);
        this.notificationForm.controls.vods.setErrors(null);
        this.notificationForm.updateValueAndValidity();
      } else {
        const validators = [Validators.required];
        this.notificationForm.controls.categories.setValidators(validators);
        this.notificationForm.controls.vods.setValidators(validators);
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
      const vods = this.notificationForm.controls.vods.value.map(el =>
        this.vodList.find(x => x.platformName === el)
      );
      this.notificationForm.controls.categories.setValue(categories);
      this.notificationForm.controls.vods.setValue(vods);

      this.notification = this.notificationForm.value;

      // populate dropdown lists back 
      const selectedCategories = this.notification.categories.map(cat => cat.categoryName);
      const selectedVods = this.notification.vods.map(vod => vod.platformName);
      this.notificationForm.controls.categories.setValue(selectedCategories);
      this.notificationForm.controls.vods.setValue(selectedVods);
    }
    else {
      this.notification = this.notificationForm.value;
    }
    this.notification.date = new Date();
    console.log(this.notification)
    this.notificationService.createNotification(this.notification).subscribe(res => {
      // if (res['status'] == 1) {
      //   this.openSnackBar(res['messages'], 'Zamknij', 'red-snackbar');
      // } else {
      //   this.openSnackBar(res['messages'], 'Zamknij', 'green-snackbar');
      // }
      console.log(res);
    });
  }
}
