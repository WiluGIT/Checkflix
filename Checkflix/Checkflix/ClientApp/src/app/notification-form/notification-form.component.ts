import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
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
  constructor(private fb: FormBuilder,
    private vodService: VodService,
    private categorySevice: CategoryService) { }

  ngOnInit() {
    this.notificationForm = this.fb.group({
      content: ['',
        [Validators.required
        ]],
      categories: ['', [
        Validators.required
      ]],
      vods: ['', [
        Validators.required
      ]]
    });

    this.vodService
      .getVods()
      .subscribe(vods => this.vodList = vods);

    this.categorySevice
      .getCategories()
      .subscribe(categories => this.categoryList = categories);
  }

  get content() {
    return this.notificationForm.get('content');
  }

  submitForm() {
    // convert categories, vods dropdownlist value to proper object and set to form value
    const categories = this.notificationForm.controls.categories.value.map(el =>
      this.categoryList.find(x => x.categoryName === el)
    );
    const vods = this.notificationForm.controls.vods.value.map(el =>
      this.vodList.find(x => x.platformName === el)
    );

    this.notificationForm.controls.categories.setValue(categories);
    this.notificationForm.controls.vods.setValue(vods);

    // bind values from form to API POST/PUT
    this.notification = this.notificationForm.value;
    this.notification.date = new Date();
    console.log(this.notification)
    // this.productionService.createProduction(this.production).subscribe(res => {
    //   if (res['status'] == 1) {
    //     this.openSnackBar(res['messages'], 'Zamknij', 'red-snackbar');
    //   } else {
    //     this.openSnackBar(res['messages'], 'Zamknij', 'green-snackbar');
    //   }

      // populate dropdown lists back 
      const selectedCategories = this.notification.categories.map(cat => cat.categoryName);
      const selectedVods = this.notification.vods.map(vod => vod.platformName);
      this.notificationForm.controls.categories.setValue(selectedCategories);
      this.notificationForm.controls.vods.setValue(selectedVods);
    }
}
